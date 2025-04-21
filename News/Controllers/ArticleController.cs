using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using News.Models;

namespace WebApi.Controllers;

public class ArticleController: BaseController
{
    [HttpGet]
    public List<ArticleModel> Get()
    {
        var path = Path.Combine($"{Directory.GetCurrentDirectory()}.Infrastructure", "Articles.csv");

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<ArticleModel>().Take(10).ToList();
        var filtered = records
            .Where(r =>
                !string.IsNullOrWhiteSpace(r.Title) &&
                !string.IsNullOrWhiteSpace(r.Description)
            )
            .ToList();
        return filtered;
    } 
}