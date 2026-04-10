namespace News.Models;

public class EntityModel
{
    public string Name { get; set; } = null!;
    public string? Sentiment { get; set; }
    public List<string> Tickers { get; set; } = new();
}