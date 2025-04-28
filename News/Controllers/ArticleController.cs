using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Article = News.Models.Article;

namespace WebApi.Controllers;

public class ArticleController: BaseController
{
    private readonly IHttpClientFactory httpClientFactory;
    private const string ApiKey = "cf199e18af1346f0b639c47d34607f31";
    public ArticleController(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }
    
    [HttpPost]
    public async Task<IActionResult> WriteToFile() {
        var newsApiClient = new NewsApiClient(ApiKey);
        var allArticles = new List<Article>();
        int totalNeeded = 150;
        int page = 1;

        while (allArticles.Count < totalNeeded)
        {
            var response = newsApiClient.GetTopHeadlines(new TopHeadlinesRequest
            {
                Category = Categories.Science,
                Language = Languages.EN,
                PageSize = 100,
                Page = page
            });

            if (response.Status != Statuses.Ok || response.Articles == null)
            {
                return StatusCode(500, $"Error fetching data: {response.Error?.Message}");
            }

            var batch = response.Articles
                .Take(totalNeeded - allArticles.Count)
                .Select(a => new Article
                {
                    Id = Guid.NewGuid().ToString(),
                    Source = new News.Models.Source
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = a.Source?.Name
                    },
                    Author = a.Author,
                    Title = a.Title,
                    Description = a.Description,
                    Url = a.Url,
                    UrlToImage = a.UrlToImage,
                    PublishedAt = a.PublishedAt?.ToString("o"),
                    Content = a.Content,
                    Keywords = new List<string>()
                })
                .ToList();

            allArticles.AddRange(batch);

            if (response.Articles.Count < 100)
                break;

            page++;
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Articles.csv");
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        var csv = new StringBuilder();
        csv.AppendLine("Id,Title,Author,SourceId,SourceName,PublishedAt,Url,Description,Content,UrlToImage");

        foreach (var a in allArticles) {
            csv.AppendLine($"\"{a.Id}\",\"{Escape(a.Title)}\",\"{Escape(a.Author)}\",\"{a.Source.Id}\",\"{Escape(a.Source.Name)}\",\"{a.PublishedAt}\",\"{a.Url}\",\"{Escape(a.Description)}\",\"{Escape(a.Content)}\",\"{a.UrlToImage}\"");
        }

        await System.IO.File.WriteAllTextAsync(path, csv.ToString());

        return Ok(new { Saved = allArticles.Count, Path = path });
    }

    private static string Escape(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? "" : input.Replace("\"", "\"\"");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Articles.csv");

        if (!System.IO.File.Exists(path)) {
            return NotFound();
        }

        var lines = await System.IO.File.ReadAllLinesAsync(path);

        var articles = new List<Article>();

        for (int i = 1; i < lines.Length; i++)
        {
            var fields = ParseCsvLine(lines[i]);
            if (fields.Length < 10)
                continue;

            articles.Add(new Article
            {
                Id = fields[0],
                Title = fields[1],
                Author = fields[2],
                Source = new News.Models.Source
                {
                    Id = fields[3],
                    Name = fields[4]
                },
                PublishedAt = fields[5],
                Url = fields[6],
                Description = fields[7],
                Content = fields[8],
                UrlToImage = fields[9],
                Keywords = new List<string>() 
            });
        }

        return Ok(articles);
    }

    [HttpGet]
    public async Task<IActionResult> Get(string id)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Articles.csv");

        if (!System.IO.File.Exists(path))
        {
            return NotFound("CSV файл не найден.");
        }

        var lines = await System.IO.File.ReadAllLinesAsync(path);

        // Перебираем строки, пропуская заголовок
        for (int i = 1; i < lines.Length; i++)
        {
            var fields = ParseCsvLine(lines[i]);
            if (fields.Length < 10)
                continue;

            if (fields[0] == id) // Сравниваем по ID
            {
                var article = new Article
                {
                    Id = fields[0],
                    Title = fields[1],
                    Author = fields[2],
                    Source = new News.Models.Source
                    {
                        Id = fields[3],
                        Name = fields[4]
                    },
                    PublishedAt = fields[5],
                    Url = fields[6],
                    Description = fields[7],
                    Content = fields[8],
                    UrlToImage = fields[9],
                    Keywords = new List<string>()
                };

                return Ok(article);
            }
        }

        return NotFound($"Статья с Id '{id}' не найдена.");
    }

    private string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        values.Add(current.ToString());

        return values.ToArray();
    }

    
    /*[HttpGet("get-top-headlines")]
    public IActionResult GetTopHeadlines()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Articles.csv");

        // Проверяем, существует ли файл
        if (!System.IO.File.Exists(path))
        {
            return NotFound($"File not found at {path}");
        }

        // Чтение данных из CSV файла
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // Чтение всех записей и преобразование их в список Article
        var records = csv.GetRecords<Article>().ToList();

        if (records.Count == 0)
        {
            return NotFound("No articles found in the CSV file.");
        }

        // Фильтруем статьи, чтобы исключить пустые или некорректные записи
        var filteredArticles = records
            .Where(r => !string.IsNullOrWhiteSpace(r.Title) && !string.IsNullOrWhiteSpace(r.Description))
            .Take(10)
            .ToList();

        return Ok(filteredArticles);
    }*/
}