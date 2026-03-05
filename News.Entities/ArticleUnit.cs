namespace News.Entities;

public class ArticleUnit: BaseEntity
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!; 
    public string? Sentiment { get; set; }
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;
}