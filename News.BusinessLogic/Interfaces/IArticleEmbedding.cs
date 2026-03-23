using News.Entities;
using Pgvector;

namespace News.BusinessLogic.Interfaces;

public interface IArticleEmbedding
{
    public string ModelName { get; set; }
    public int Dimensions { get; set; }
    public Vector Vector { get; set; }
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
    public DateTime CreatedAt { get; set; }
}