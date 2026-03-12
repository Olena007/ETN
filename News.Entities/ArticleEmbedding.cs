using Pgvector;

namespace News.Entities;

public class ArticleEmbedding: BaseEntity
{
    public string ModelName { get; set; } = null!;   // "all-MiniLM-L6-v2"
    public int Dimensions { get; set; }               // 384
    public Vector  Vector { get; set; } = null!;
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

}