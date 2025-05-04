using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json.Linq;
using Article = News.Models.Article;

namespace WebApi.Controllers;

public class ArticleController: BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = System.IO.File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<Article>>();

        return Ok(articles);
    }
    
    [HttpGet("uri")]
    public IActionResult Get(string uri)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = System.IO.File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<Article>>();
        
        var article = articles?.FirstOrDefault(x => x.Uri == uri);

        return Ok(article);
    }
    
    [HttpGet]
    public IActionResult GetRecommended(string uri)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "articles.json");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл articles.json не найден.");

        var json = System.IO.File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);

        var articles = jObject["articles"]?["results"]?.ToObject<List<Article>>();

        if (articles == null || !articles.Any())
        {
            return NotFound(new { message = "Articles not found." });
        }

        var currentArticle = articles.FirstOrDefault(a => a.Uri == uri);
        if (currentArticle == null)
        {
            return NotFound(new { message = "Article not found." });
        }

        var recommendedArticles = articles
            .Where(a => a.Uri != uri && a.Sim >= 0.75) 
            .OrderByDescending(a => a.Sim)
            .Take(5)
            .ToList();

        if (recommendedArticles.Count == 0)
        {
            return NotFound(new { message = "No similar articles found." });
        }

        return Ok(recommendedArticles);
    }
}