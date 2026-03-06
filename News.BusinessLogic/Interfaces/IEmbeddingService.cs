namespace News.BusinessLogic.Interfaces;

//Generates embedding
public interface IEmbeddingService
{
    
    /// <summary>
    /// Generates embedding vector for text through Python microservice
    /// </summary>
    Task<float[]> GenerateAsync(string text, CancellationToken ct = default);

    /// <summary>
    /// Generates embedding batch - for multiple articles
    /// </summary>
    Task<float[][]> GenerateBatchAsync(IEnumerable<string> texts, CancellationToken ct = default);
}