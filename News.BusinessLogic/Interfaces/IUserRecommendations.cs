using News.BusinessLogic.Articles;
using News.Entities;

namespace News.BusinessLogic.Interfaces;

public interface IUserRecommendations
{
    /// <summary>
    ///     Tracks when a user views an article
    /// </summary>
    Task TrackViewAsync(Guid userId, Guid articleId, CancellationToken ct = default);

    /// <summary>
    ///     Returns personalized recommendations based on reading history
    /// </summary>
    Task<List<GetArticles.ArticleListItemDto>> GetRecommendationsAsync(Guid userId, int topN = 10, CancellationToken ct = default);
}