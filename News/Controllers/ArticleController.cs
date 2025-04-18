using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;

public class ArticleController: BaseController
{
    public class ArticleModel
    {
        public string Article {  get; set; }
        public string Heading { get; set; }
        public DateTime Date { get; set; }
        public string NewsType { get; set; }
    }
    
    [HttpGet]

    public List<ArticleModel> Get()
    {
        var path = Path.Combine($"{Directory.GetCurrentDirectory()}.Infrastructure", "Articles.csv");

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<ArticleModel>().ToList();
        return records;
    } 
}