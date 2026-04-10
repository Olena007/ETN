namespace News.Models;

public class ArticleModel
{
    public string Uuid { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string? Author { get; set; }
    public DateTime Published { get; set; }
    public string? Language { get; set; }
    public string? Sentiment { get; set; }
    public string? Url { get; set; }
    public ThreadModel? Thread { get; set; }
    public List<string> Categories { get; set; } = new();
    public EntitiesModel? Entities { get; set; }
}