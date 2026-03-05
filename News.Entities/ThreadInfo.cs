namespace News.Entities;

public class ThreadInfo: BaseEntity
{
    public string Site { get; set; }
    public string? Country { get; set; }
    public string? MainImage { get; set; }
    public int? DomainRank { get; set; }
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;
}