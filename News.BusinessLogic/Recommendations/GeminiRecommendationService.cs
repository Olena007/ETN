using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.Entities;
using News.Models;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace News.BusinessLogic.Recommendations;

public class GeminiRecommendationService(
    IEmbeddingService embeddings,
    INewsDbContext db) : IRecommendationService
{
    public async Task IndexArticleAsync(Guid articleId, CancellationToken ct = default)
    {
        var existing = await db.ArticleEmbeddingsGemini
            .FirstOrDefaultAsync(e => e.ArticleId == articleId, ct);

        if (existing is not null) return;

        var article = await db.Articles.FindAsync([articleId], ct)
                      ?? throw new KeyNotFoundException($"Article {articleId} not found");

        var floats = await embeddings.GenerateAsync($"{article.Title}. {article.Text}", ct);
        var vector = new Vector(floats);

        db.ArticleEmbeddingsGemini.Add(new ArticleEmbeddingGemini
        {
            ArticleId = articleId,
            ModelName = "text-embedding-004",
            Dimensions = floats.Length,
            Vector = vector
        });

        await db.SaveChangesAsync(ct);
    }

    public async Task<IndexArticlesModel> IndexArticlesAsync(CancellationToken ct)
    {
        var articleIds = await db.Articles
            .Where(a => !db.ArticleEmbeddingsGemini.Any(e => e.ArticleId == a.Id))
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
                    Console.WriteLine($"[Gemini] Progress: {processed}/{articleIds.Count}");
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
        var c = db.ArticleEmbeddingsGemini.Count();
        var source = await db.ArticleEmbeddingsGemini
                         .FirstOrDefaultAsync(e => e.ArticleId == articleId, ct)
                     ?? throw new KeyNotFoundException($"Embedding for article {articleId} not found.");

        return await db.ArticleEmbeddingsGemini
            .Where(e => e.ArticleId != articleId)
            .OrderBy(e => e.Vector.CosineDistance(source.Vector))
            .Take(topN)
            .Include(e => e.Article)
            .Select(e => e.Article)
            .ToListAsync(ct);
    }
}