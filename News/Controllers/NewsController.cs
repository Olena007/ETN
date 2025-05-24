using Microsoft.AspNetCore.Mvc;
using News.BusinessLogic.Interfaces;
using News.BusinessLogic.News;
using News.Entities;
using Newtonsoft.Json.Linq;
using NewsModel = News.BusinessLogic.News.NewsModel;

namespace WebApi.Controllers;

public class NewsController(INewsDbContext context) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] GetNewsQuery query)
    {
        var result = await Mediator.Send(query);
        if (result.News == null || !result.News.Any())
            return NotFound(); 

        return Ok(result); 
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<NewsModel>> Get(Guid id)
    {
        var query = new GetSingleNewsQuery()
        {
            NewsId = id
        };

        return Ok(await Mediator.Send(query));
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

        var articles = jObject["articles"]?["results"]?.ToObject<List<NewsJsonDto>>();

        if (articles == null || !articles.Any())
            return BadRequest("Нет данных для импорта.");

        context.Authors.RemoveRange(context.Authors);
        context.Categories.RemoveRange(context.Categories);
        context.Videos.RemoveRange(context.Videos);
        context.Views.RemoveRange(context.Views);
        context.News.RemoveRange(context.News);
        await context.SaveChangesAsync(token);

        foreach (var dto in articles)
        {
            var newsId = Guid.NewGuid();
            var news = new News.Entities.News
            {
                NewsId = newsId,
                Uri = dto.Uri,
                Lang = dto.Lang,
                Date = dto.Date,
                Time = dto.Time,
                DateTime = dto.DateTime,
                Sim = dto.Sim,
                Url = dto.Url,
                Title = dto.Title,
                Body = dto.Body,
                Links = dto.Links ?? new List<string>(),
                Image = dto.Image ?? "",
                EventUri = dto.EventUri,
                Authors = dto.Authors?.Select(a => new Author
                {
                    AuthorId = Guid.NewGuid(),
                    NewsId = newsId,
                    Uri = a.Uri,
                    Name = a.Name,
                    Type = a.Type,
                    IsAgency = a.IsAgency
                }).ToList() ?? new List<Author>(),

                Categories = dto.Categories?.Select(c => new Category
                {
                    CategoryId = Guid.NewGuid(),
                    NewsId = newsId,
                    Uri = c.Uri,
                    Label = c.Label,
                    Wgt = c.Wgt
                }).ToList() ?? new List<Category>(),

                Source = dto.Source != null
                    ? new List<Source>
                    {
                        new()
                        {
                            SourceId = Guid.NewGuid(),
                            NewsId = newsId,
                            Uri = dto.Source.Uri,
                            DataType = dto.Source.DataType,
                            Title = dto.Source.Title,
                            Description = dto.Source.Description,
                            LocationValidated = false
                        }
                    }
                    : new List<Source>()
            };

            context.News.Add(news);
        }

        await context.SaveChangesAsync(token);

        return Ok($"{articles.Count} статей импортировано.");
    }
}

public class NewsJsonDto
{
    public string Uri { get; set; }
    public string Lang { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public double Sim { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> Links { get; set; }
    public string? Image { get; set; }
    public string? EventUri { get; set; }
    public List<AuthorJsonDto> Authors { get; set; }
    public List<CategoryJsonDto> Categories { get; set; }
    public List<VideoJsonDto> Videos { get; set; } // если будут
    public SourceJsonDto Source { get; set; }
}

public class AuthorJsonDto
{
    public string Uri { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsAgency { get; set; }
}

public class CategoryJsonDto
{
    public string Uri { get; set; }
    public string Label { get; set; }
    public int Wgt { get; set; }
}

public class SourceJsonDto
{
    public string Uri { get; set; }
    public string DataType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

public class VideoJsonDto
{
    public string Uri { get; set; }
    public string? Label { get; set; }
}