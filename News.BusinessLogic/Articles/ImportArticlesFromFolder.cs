using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.Articles;

public class ImportArticlesFromFolder
{
    public class ImportArticlesCommand : IRequest<ImportResult>
    {
        public string FolderPath { get; set; } = null!;
    }

    public class ImportResult
    {
        public int TotalFiles { get; set; }
        public int Imported { get; set; }
        public int Skipped { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class EntitiesData
    {
        public List<EntityItem> Persons { get; set; } = new();
        public List<EntityItem> Organizations { get; set; } = new();
        public List<EntityItem> Locations { get; set; } = new();
    }

    public class EntityItem
    {
        public string Name { get; set; } = null!;
        public string? Sentiment { get; set; }
    }

    public class ArticleFileData
    {
        public string Uuid { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Author { get; set; }
        public DateTime Published { get; set; }
        public string? Language { get; set; }
        public string? Sentiment { get; set; }
        public string? Url { get; set; }
        public ThreadInfoData Thread { get; set; } = null!;
        public List<string> Categories { get; set; } = new();
        public EntitiesData Entities { get; set; } = new();
    }

    public class ThreadInfoData
    {
        public string Site { get; set; } = null!;
        public string? Country { get; set; }
        public string? MainImage { get; set; }
        public int? DomainRank { get; set; }
    }

    public class ImportArticlesCommandHandler : IRequestHandler<ImportArticlesCommand, ImportResult>
    {
        private const int BATCH_SIZE = 100;
        private readonly INewsDbContext _context;
        private readonly DbContext _dbContext;

        public ImportArticlesCommandHandler(INewsDbContext context)
        {
            _context = context;
            _dbContext = (DbContext)context;
        }

        public async Task<ImportResult> Handle(ImportArticlesCommand request, CancellationToken cancellationToken)
        {
            var result = new ImportResult();

            if (!Directory.Exists(request.FolderPath))
                throw new EntryPointNotFoundException($"Folder {request.FolderPath} not found");

            var jsonFiles = Directory.GetFiles(request.FolderPath, "*.json", SearchOption.AllDirectories);
            result.TotalFiles = jsonFiles.Length;

            var existingUuids = new HashSet<string>(
                await _context.Articles.Select(a => a.Uuid).ToListAsync(cancellationToken)
            );

            var categoriesCache = await _context.Categories
                .ToDictionaryAsync(c => c.Name, c => c, cancellationToken);

            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            try
            {
                var articlesToAdd = new List<Article>();
                var categoriesToAdd = new Dictionary<string, Category>();
                var uuidsInCurrentBatch = new HashSet<string>();

                foreach (var filePath in jsonFiles)
                    try
                    {
                        var article = await ParseArticleFromFile(filePath, existingUuids, uuidsInCurrentBatch,
                            categoriesCache, categoriesToAdd, cancellationToken);

                        if (article != null)
                        {
                            articlesToAdd.Add(article);
                            result.Imported++;

                            uuidsInCurrentBatch.Add(article.Uuid);
                            existingUuids.Add(article.Uuid);

                            if (articlesToAdd.Count >= BATCH_SIZE)
                            {
                                await SaveBatch(articlesToAdd, categoriesToAdd.Values.ToList(), cancellationToken);
                                articlesToAdd.Clear();
                                categoriesToAdd.Clear();
                                uuidsInCurrentBatch.Clear();
                            }
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

                if (articlesToAdd.Any())
                    await SaveBatch(articlesToAdd, categoriesToAdd.Values.ToList(), cancellationToken);
            }
            finally
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            return result;
        }

        private async Task<Article?> ParseArticleFromFile(
            string filePath,
            HashSet<string> existingUuids,
            HashSet<string> uuidsInCurrentBatch,
            Dictionary<string, Category> existingCategories,
            Dictionary<string, Category> newCategoriesInBatch,
            CancellationToken cancellationToken)
        {
            var jsonContent = await File.ReadAllTextAsync(filePath, cancellationToken);
            var articleData = JsonSerializer.Deserialize<ArticleFileData>(jsonContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (articleData == null)
                throw new Exception("Failed to deserialize article data");

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
                    Site = articleData.Thread.Site,
                    Country = articleData.Thread.Country,
                    MainImage = articleData.Thread.MainImage,
                    DomainRank = articleData.Thread.DomainRank,
                    CreatedAt = DateTime.UtcNow
                };

            foreach (var categoryName in articleData.Categories)
            {
                Category? category = null;

                if (newCategoriesInBatch.TryGetValue(categoryName, out var newCat))
                {
                    category = newCat;
                }
                else if (existingCategories.TryGetValue(categoryName, out var existingCat))
                {
                    category = existingCat;
                }
                else
                {
                    category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = categoryName,
                        CreatedAt = DateTime.UtcNow
                    };
                    newCategoriesInBatch[categoryName] = category;
                    existingCategories[categoryName] = category;
                }

                article.Categories.Add(category);
            }

            if (articleData.Entities != null)
            {
                var entities = new List<ArticleUnit>();

                foreach (var person in articleData.Entities.Persons)
                    entities.Add(new ArticleUnit
                    {
                        Name = person.Name,
                        Type = "person",
                        Sentiment = person.Sentiment
                    });

                foreach (var org in articleData.Entities.Organizations)
                    entities.Add(new ArticleUnit
                    {
                        Name = org.Name,
                        Type = "organization",
                        Sentiment = org.Sentiment
                    });

                foreach (var location in articleData.Entities.Locations)
                    entities.Add(new ArticleUnit
                    {
                        Name = location.Name,
                        Type = "location",
                        Sentiment = location.Sentiment
                    });

                article.Entities = entities;
            }

            return article;
        }

        private async Task SaveBatch(List<Article> articles, List<Category> newCategories,
            CancellationToken cancellationToken)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (newCategories.Any())
                {
                    var distinctCategories = newCategories
                        .GroupBy(c => c.Name)
                        .Select(g => g.First())
                        .ToList();

                    await _context.Categories.AddRangeAsync(distinctCategories, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                await _context.Articles.AddRangeAsync(articles, cancellationToken);
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