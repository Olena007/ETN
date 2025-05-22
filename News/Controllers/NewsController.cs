using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.BusinessLogic.News;
using News.Entities;
using News.Models;
using Newtonsoft.Json.Linq;
using Location = News.Entities.Location;

namespace WebApi.Controllers;

public class NewsController(INewsDbContext context) : BaseController
{
    [HttpPost("getAll")]
    public async Task<ActionResult<NewsVm>> GetAll([FromBody] GetNewsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("get")]
    public IActionResult Get()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = System.IO.File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<NewsModel>>();

        return Ok(articles);
    }

    [HttpGet("by-uri")]
    public IActionResult GetByUri(string uri)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = System.IO.File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<NewsModel>>();

        var article = articles?.FirstOrDefault(x => x.Uri == uri);

        return Ok(article);
    }

    [HttpGet("recommended")]
    public IActionResult GetRecommended(string uri)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = System.IO.File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<NewsModel>>();

        if (articles == null || !articles.Any()) return NotFound(new { message = "Articles not found." });

        var currentArticle = articles.FirstOrDefault(a => a.Uri == uri);
        if (currentArticle == null) return NotFound(new { message = "Article not found." });

        var recommendedArticles = articles
            .Where(a => a.Uri != uri && a.Sim >= 0.75)
            .OrderByDescending(a => a.Sim)
            .Take(5)
            .ToList();

        if (recommendedArticles.Count == 0) return NotFound(new { message = "No similar articles found." });

        return Ok(recommendedArticles);
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportToDatabase(CancellationToken token)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = await System.IO.File.ReadAllTextAsync(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<News.Entities.News>>();

        if (articles == null || !articles.Any())
            return BadRequest("Нет данных для импорта.");
        
        context.Authors.RemoveRange(context.Authors);
        context.Categories.RemoveRange(context.Categories);
        context.Videos.RemoveRange(context.Videos);
        context.Views.RemoveRange(context.Views);
        context.News.RemoveRange(context.News);
        await context.SaveChangesAsync(token);
        
        var existingCategories = await context.Categories.ToDictionaryAsync(c => c.Uri, token);
        
        foreach (var article in articles)
        {
            var exists = await context.News.AnyAsync(a => a.Uri == article.Uri, token);
            if (exists) continue;

            article.Image ??= "";

            context.News.Add(article);
        }

        await context.SaveChangesAsync(token);

        return Ok($"{articles.Count} статей импортировано.");
    }
}