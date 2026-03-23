using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.Entities;
using News.Models;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace News.BusinessLogic.Recommendations;

public abstract class RecommendationBase<TEmbedding>(
    IEmbeddingService embeddings,
    INewsDbContext db) : IRecommendationService
    where TEmbedding : class, IArticleEmbedding
{
    protected abstract string ModelName { get; }
    protected abstract DbSet<TEmbedding> Table { get; }

    public async Task IndexArticleAsync(Guid articleId, CancellationToken ct = default)
    {
        var article = await db.Articles.FindAsync([articleId], ct)
                      ?? throw new KeyNotFoundException($"Article {articleId} not found");

        var floats = await embeddings.GenerateAsync($"{article.Title}. {article.Text}", ct);
        var vector = new Vector(floats);

        var existing = await Table.FirstOrDefaultAsync(e => e.ArticleId == articleId, ct);
        if (existing is not null)
        {
            existing.Vector = vector;
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            Table.Add(CreateEmbedding(articleId, vector, floats.Length));
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task<IndexArticlesModel> IndexArticlesAsync(CancellationToken ct)
    {
        var articleIds = await db.Articles
            .Where(a => !Table.Any(e => e.ArticleId == a.Id))
            .Select(a => a.Id)
            .ToListAsync(ct);

        var processed = 0;
        var failed = new List<Guid>();

        foreach (var id in articleIds)
            try
            {
                await IndexArticleAsync(id, ct);
                processed++;
                if (processed % 100 == 0)
                    Console.WriteLine($"[{ModelName}] Progress: {processed}/{articleIds.Count}");
            }
            catch (Exception ex)
            {
                failed.Add(id);
                Console.WriteLine($"Failed {id}: {ex.Message}");
            }

        return new IndexArticlesModel
        {
            TotalCount = articleIds.Count,
            Processed = processed,
            FailedCount = failed.Count,
            FailedIds = failed
        };
    }

    public async Task<IEnumerable<Article>> GetSimilarAsync(Guid articleId, int topN = 5,
        CancellationToken ct = default)
    {
        var source = await Table.FirstOrDefaultAsync(e => e.ArticleId == articleId, ct)
                     ?? throw new KeyNotFoundException($"Embedding for article {articleId} not found.");

        var vector = source.Vector;

        return await Table
            .Where(e => e.ArticleId != articleId)
            .OrderBy(e => e.Vector.CosineDistance(vector))
            .Take(topN)
            .Include(e => e.Article)
            .Select(e => e.Article)
            .ToListAsync(ct);
    }

    protected abstract TEmbedding CreateEmbedding(Guid articleId, Vector vector, int dimensions);
}