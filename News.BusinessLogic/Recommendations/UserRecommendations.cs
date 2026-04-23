using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Articles;
using News.BusinessLogic.Interfaces;
using News.Entities;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace News.BusinessLogic.Recommendations;

public class UserRecommendations(INewsDbContext db) : IUserRecommendations
{
    public async Task TrackViewAsync(Guid userId, Guid articleId, CancellationToken ct = default)
    {
        var alreadyViewed = await db.UserArticleViews
            .AnyAsync(v => v.UserId == userId
                           && v.ArticleId == articleId
                           && v.ViewedAt.Date == DateTime.UtcNow.Date, ct);

        if (alreadyViewed) return;

        db.UserArticleViews.Add(new UserArticleView
        {
            UserId = userId,
            ArticleId = articleId
        });

        await db.SaveChangesAsync(ct);
    }  

    public async Task<List<GetArticles.ArticleListItemDto>> GetRecommendationsAsync(Guid userId, int topN = 10,
        CancellationToken ct = default)
    {
        var recentArticleIds = await db.UserArticleViews
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.ViewedAt)
            .Take(10)
            .Select(v => v.ArticleId)
            .ToListAsync(ct);

        if (recentArticleIds.Count == 0)
            return [];

        var vectors = await db.ArticleEmbeddings
            .Where(e => recentArticleIds.Contains(e.ArticleId))
            .Select(e => e.Vector)
            .ToListAsync(ct);

        if (vectors.Count == 0)
            return [];

        var avgVector = new Vector(AverageVectors(vectors));

        var articles = await db.ArticleEmbeddings
            .Where(e => !recentArticleIds.Contains(e.ArticleId))
            .OrderBy(e => e.Vector.CosineDistance(avgVector))
            .Take(topN)
            .Include(e => e.Article)
            .ThenInclude(a => a.Thread)  
            .Select(e => e.Article)
            .ToListAsync(ct);
        
        var result = articles.Select(a => new GetArticles.ArticleListItemDto
        {
            Id = a.Id,
            Uuid = a.Uuid,
            Title = a.Title,
            Text = a.Text,
            Author = a.Author,
            Published = a.Published,
            Language = a.Language,
            Thread = a.Thread == null ? null : new GetArticle.ThreadInfoDto
            {
                Id = a.Thread.Id,
                MainImage = a.Thread.MainImage,
                Site = a.Thread.Site,
                Country = a.Thread.Country
            }
        }).ToList();

        return result;
    }

    private static float[] AverageVectors(List<Vector> vectors)
    {
        var dims = vectors[0].Memory.Length;
        var result = new float[dims];

        foreach (var v in vectors)
            for (var i = 0; i < dims; i++)
                result[i] += v.Memory.Span[i];

        for (var i = 0; i < dims; i++)
            result[i] /= vectors.Count;

        return result;
    }
}