namespace News.Entities;

public class Article: BaseEntity
{
    public string Uuid { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string? Author { get; set; }
    public DateTime Published { get; set; }
    public string? Language { get; set; }
    public string? Sentiment { get; set; }
    public string? Url { get; set; }
    public ThreadInfo Thread { get; set; } = null!;
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<ArticleUnit> Entities { get; set; } = new List<ArticleUnit>();
}