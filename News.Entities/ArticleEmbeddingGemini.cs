using Pgvector;

namespace News.Entities;

//Entity for Gemini embeding with 768 dimension 
public class ArticleEmbeddingGemini : BaseEntity
{
    public string ModelName { get; set; } = null!;  
    public int Dimensions { get; set; }               
    public Vector  Vector { get; set; } = null!;
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;
}