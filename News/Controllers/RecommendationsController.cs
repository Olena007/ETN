using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Interfaces;

namespace WebApi.Controllers;

public class RecommendationsController(IRecommendationService recommendations) : BaseController
{
    /// <summary>
    /// GET /api/articles/42/recommendations?topN=5
    /// Returns similar articles upon cosyne similarity of embeddings
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSimilar(Guid articleId, [FromQuery] int topN = 5)
    {
        var articles = await recommendations.GetSimilarAsync(articleId, topN);
        return Ok(articles);
    }

    /// <summary>
    /// POST /api/articles/42/recommendations/index
    /// Generates and saves embedding for the article
    /// </summary>
    [HttpPost("index")]
    public async Task<IActionResult> Index(Guid articleId)
    {
        await recommendations.IndexArticleAsync(articleId);
        return Ok(new { message = $"Article {articleId} indexed successfully" });
    }
}