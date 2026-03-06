using News.Entities;

namespace News.BusinessLogic.Interfaces;

public interface IRecommendationService
{
    /// <summary>
    /// Returns topN articles similar to the set cosyn similarity
    /// </summary>
    Task<IEnumerable<Article>> GetSimilarAsync(Guid articleId, int topN = 5, CancellationToken ct = default);

    /// <summary>
    ///  Generates and saves embedding for the article (calla upon publish)
    /// </summary>
    Task IndexArticleAsync(Guid articleId, CancellationToken ct = default);
}