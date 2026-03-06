using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.BusinessLogic.Embedding;

public class RecommendationService(IEmbeddingService embeddings, INewsDbContext db) : IRecommendationService
{
    public async Task IndexArticleAsync(Guid articleId, CancellationToken ct = default)
    {
        var article = await db.Articles.FindAsync([articleId], ct)
            ?? throw new KeyNotFoundException($"Article {articleId} not found");

        // Объединяем заголовок и текст для лучшего эмбеддинга
        var input = $"{article.Title}. {article.Text}";
        var vector = await embeddings.GenerateAsync(input, ct);

        // Upsert: обновляем если уже есть
        var existing = await db.ArticleEmbeddings
            .FirstOrDefaultAsync(e => e.ArticleId == articleId, ct);

        if (existing is not null)
        {
            existing.Vector = vector;
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            db.ArticleEmbeddings.Add(new ArticleEmbedding
            {
                ArticleId = articleId,
                ModelName = "all-MiniLM-L6-v2",
                Dimensions = vector.Length,
                Vector = vector
            });
        }

        await db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Article>> GetSimilarAsync(Guid articleId, int topN = 5, CancellationToken ct = default)
    {
        var sourceEmbedding = await db.ArticleEmbeddings
            .FirstOrDefaultAsync(e => e.ArticleId == articleId, ct)
            ?? throw new KeyNotFoundException($"Embedding for article {articleId} not found. Run IndexArticleAsync first.");

        // Загружаем все эмбеддинги кроме текущей статьи
        // Для больших БД — заменить на pgvector запрос (см. комментарий ниже)
        var allEmbeddings = await db.ArticleEmbeddings
            .Where(e => e.ArticleId != articleId)
            .Include(e => e.Article)
            .ToListAsync(ct);

        var similarities = allEmbeddings
            .Select(e => (
                Article: e.Article,
                Score: CosineSimilarity(sourceEmbedding.Vector, e.Vector)
            ))
            .OrderByDescending(x => x.Score)
            .Take(topN)
            .Select(x => x.Article);

        return similarities;

        /*
         * pgvector вариант (когда подключишь расширение):
         * 
         * var vector = sourceEmbedding.Vector;
         * return await _db.Set<Article>()
         *     .FromSql($"""
         *         SELECT a.* FROM articles a
         *         JOIN article_embeddings e ON e.article_id = a.id
         *         ORDER BY e.vector <=> {vector}::vector
         *         LIMIT {topN}
         *     """)
         *     .ToListAsync(ct);
         */
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0, normA = 0, normB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }
        return dot / (MathF.Sqrt(normA) * MathF.Sqrt(normB) + float.Epsilon);
    }
}