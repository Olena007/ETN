using System.Net.Http.Json;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Embeddings;

public class SentenceTransformerEmbedding(HttpClient http) : IEmbeddingService
{
    public async Task<float[]> GenerateAsync(string text, CancellationToken ct = default)
    {
        var response = await http.PostAsJsonAsync("/embed", new { text }, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<EmbedResponse>(ct);
        return result!.Embedding;
    }

    public async Task<float[][]> GenerateBatchAsync(IEnumerable<string> texts, CancellationToken ct = default)
    {
        var response = await http.PostAsJsonAsync("/embed/batch", new { texts }, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<EmbedBatchResponse>(ct);
        return result!.Embeddings;
    }

    private record EmbedResponse(float[] Embedding);

    private record EmbedBatchResponse(float[][] Embeddings);
}