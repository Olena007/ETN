namespace News.Entities;

public class ArticleEmbedding: BaseEntity
{
    public string ModelName { get; set; } = null!;   // "all-MiniLM-L6-v2"
    public int Dimensions { get; set; }               // 384
    public float[] Vector { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

}