using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.Entities;
using News.Models;

namespace News.BusinessLogic.Articles;

public class ImportArticlesFromFolder
{
    public class ImportArticlesCommand : IRequest<ImportResultModel>
    {
        public string FolderPath { get; set; } = null!;
    }

    public class ImportArticlesCommandHandler : IRequestHandler<ImportArticlesCommand, ImportResultModel>
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private readonly INewsDbContext _context;
        private readonly DbContext _dbContext;

        public ImportArticlesCommandHandler(INewsDbContext context)
        {
            _context = context;
            _dbContext = (DbContext)context;
        }

        public async Task<ImportResultModel> Handle(ImportArticlesCommand request, CancellationToken cancellationToken)
        {
            var result = new ImportResultModel();

            if (!Directory.Exists(request.FolderPath))
                throw new EntryPointNotFoundException($"Folder {request.FolderPath} not found");
            
            var jsonFiles = Directory.GetFiles(request.FolderPath, "*.json", SearchOption.AllDirectories);
            result.TotalFiles = jsonFiles.Length;

            var existingUuids = (await _context.Articles
                    .Select(a => a.Uuid)
                    .ToListAsync(cancellationToken))
                .ToHashSet();

            var categoriesCache = await _context.Categories
                .ToDictionaryAsync(c => c.Name, c => c.Id, cancellationToken);
            
            var uuidsInCurrentBatch = new HashSet<string>();

            var articlesToAdd = new List<Article>();
            var categoriesToAdd = new Dictionary<string, Guid>();

            foreach (var filePath in jsonFiles)
                try
                {
                    var article = await ParseArticleFromFile(
                        filePath,
                        existingUuids,
                        uuidsInCurrentBatch,
                        categoriesCache,
                        categoriesToAdd,
                        cancellationToken);

                    if (article != null)
                    {
                        articlesToAdd.Add(article);
                        result.Imported++;
                        uuidsInCurrentBatch.Add(article.Uuid);
                        existingUuids.Add(article.Uuid);

                        if (articlesToAdd.Count < Consts.Consts.BATCH_SIZE) continue;
                        await SaveBatch(articlesToAdd, categoriesToAdd, cancellationToken);

                        foreach (var (name, id) in categoriesToAdd)
                            categoriesCache[name] = id;

                        articlesToAdd.Clear();
                        categoriesToAdd.Clear();
                        uuidsInCurrentBatch.Clear();
                    }
                    else
                    {
                        result.Skipped++;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"File {Path.GetFileName(filePath)}: {ex.Message}");
                }

            if (articlesToAdd.Count == 0) return result;
            {
                await SaveBatch(articlesToAdd, categoriesToAdd, cancellationToken);
                foreach (var (name, id) in categoriesToAdd)
                    categoriesCache[name] = id;
            }

            return result;
        }

        private async Task<Article?> ParseArticleFromFile(
            string filePath,
            HashSet<string> existingUuids,
            HashSet<string> uuidsInCurrentBatch,
            Dictionary<string, Guid> existingCategories,
            Dictionary<string, Guid> newCategoriesInBatch, 
            CancellationToken cancellationToken)
        {
            var jsonContent = await File.ReadAllTextAsync(filePath, cancellationToken);
            var articleData = JsonSerializer.Deserialize<ArticleModel>(jsonContent, JsonOptions);

            if (articleData == null)
                throw new Exception("Failed to deserialize article data");

            if (!string.Equals(articleData.Language, "english", StringComparison.OrdinalIgnoreCase))
                return null;

            if (existingUuids.Contains(articleData.Uuid) || uuidsInCurrentBatch.Contains(articleData.Uuid))
                return null;

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Uuid = articleData.Uuid,
                Title = articleData.Title,
                Text = articleData.Text,
                Author = articleData.Author,
                Published = articleData.Published.Kind == DateTimeKind.Local
                    ? articleData.Published.ToUniversalTime()
                    : articleData.Published,
                Language = articleData.Language,
                Sentiment = articleData.Sentiment,
                Url = articleData.Url,
                CreatedAt = DateTime.UtcNow
            };

            if (articleData.Thread != null)
                article.Thread = new ThreadInfo
                {
                    Id = Guid.NewGuid(),
                    ArticleId = article.Id,
                    Site = articleData.Thread.Site ?? articleData.Thread.SiteFull ?? "unknown",
                    Country = articleData.Thread.Country,
                    MainImage = string.IsNullOrEmpty(articleData.Thread.MainImage)
                        ? null
                        : articleData.Thread.MainImage,
                    DomainRank = articleData.Thread.DomainRank,
                    CreatedAt = DateTime.UtcNow
                };

            foreach (var categoryName in articleData.Categories)
            {
                Guid categoryId;

                if (existingCategories.TryGetValue(categoryName, out var existingId))
                {
                    categoryId = existingId;
                }
                else if (newCategoriesInBatch.TryGetValue(categoryName, out var batchId))
                {
                    categoryId = batchId;
                }
                else
                {
                    categoryId = Guid.NewGuid();
                    newCategoriesInBatch[categoryName] = categoryId;
                    existingCategories[categoryName] = categoryId;
                }

                article.Categories.Add(new Category { Id = categoryId });
            }

            if (articleData.Entities == null) return article;
            var entities = articleData.Entities.Persons.Select(person => new ArticleUnit { Name = person.Name, Type = "person", Sentiment = person.Sentiment }).ToList();
            entities.AddRange(articleData.Entities.Organizations.Select(org => new ArticleUnit { Name = org.Name, Type = "organization", Sentiment = org.Sentiment }));
            entities.AddRange(articleData.Entities.Locations.Select(location => new ArticleUnit { Name = location.Name, Type = "location", Sentiment = location.Sentiment }));

            article.Entities = entities;

            return article;
        }

        private async Task SaveBatch(
            List<Article> articles,
            Dictionary<string, Guid> newCategories,
            CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var (name, id) in newCategories)
                    await _dbContext.Database.ExecuteSqlRawAsync(
                        """INSERT INTO "Categories" ("Id", "Name", "CreatedAt") VALUES ({0}, {1}, {2})""",
                        id, name, DateTime.UtcNow);


                var attachedCategories = new Dictionary<Guid, Category>();

                foreach (var article in articles)
                {
                    var categoryIds = article.Categories.Select(c => c.Id).ToList();
                    article.Categories.Clear();

                    _dbContext.Entry(article).State = EntityState.Added;
                    _dbContext.Entry(article.Thread).State = EntityState.Added;

                    foreach (var catId in categoryIds)
                    {
                        if (!attachedCategories.TryGetValue(catId, out var cat))
                        {
                            cat = new Category { Id = catId };
                            _dbContext.Attach(cat);
                            attachedCategories[catId] = cat;
                        }

                        article.Categories.Add(cat);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                _dbContext.ChangeTracker.Clear();
            }
        }
    }
}