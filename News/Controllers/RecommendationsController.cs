using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Interfaces;

namespace WebApi.Controllers;

public class RecommendationsController(IRecommendationService recommendations, IUserRecommendations userRecommendations) : BaseController
{
    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    
    /// <summary> 
    /// Returns similar articles upon cosyne similarity of embeddings
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSimilar(Guid articleId, [FromQuery] int topN = 5)
    {
        var articles = await recommendations.GetSimilarAsync(articleId, topN);
        return Ok(articles);
    }

    /// <summary>
    /// Generates and saves embedding for the article
    /// </summary>
    [HttpPost("index")]
    public async Task<IActionResult> Index(Guid articleId)
    {
        await recommendations.IndexArticleAsync(articleId);
        return Ok(new { message = $"Article {articleId} indexed successfully" });
    }
    
    /// <summary>
    /// Generates and saves embedding for the articles
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IndexAll(CancellationToken ct)
    {
        var result = await recommendations.IndexArticlesAsync(ct);
        return Ok(result);
    }
    
    /// <summary>
    /// Personalized recommendations based on users reading history
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUserRecommendations([FromQuery] int topN = 10, CancellationToken ct = default)
    {
        var articles = await userRecommendations.GetRecommendationsAsync(Guid.Parse(CurrentUserId), topN, ct);
        return Ok(articles);
    }
 
    /// <summary>
    /// Called when a user opens an article
    /// </summary>
    [HttpPost("{articleId:guid}")]
    public async Task<IActionResult> Track(Guid articleId, CancellationToken ct = default)
    {
        await userRecommendations.TrackViewAsync(Guid.Parse(CurrentUserId), articleId, ct);
        return Ok();
    }
}