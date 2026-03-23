using System.Net.Http.Json;
using System.Text.Json.Serialization;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Embedding;

public class GeminiEmbedding(HttpClient http, string apiKey) : IEmbeddingService
{
    private const string Model = "gemini-embedding-001";

    public async Task<float[]> GenerateAsync(string text, CancellationToken ct = default)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:embedContent?key={apiKey}";

        var body = new
        {
            model = $"models/{Model}",
            content = new
            {
                parts = new[] { new { text } }
            },
            taskType = "SEMANTIC_SIMILARITY"
        };

        var response = await http.PostAsJsonAsync(url, body, ct);
        //response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"Gemini API error {response.StatusCode}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<GeminiEmbedResponse>(ct);
        return result!.Embedding.Values;
    }

    public async Task<float[][]> GenerateBatchAsync(IEnumerable<string> texts, CancellationToken ct = default)
    {
        var results = new List<float[]>();
        foreach (var text in texts) results.Add(await GenerateAsync(text, ct));
        return results.ToArray();
    }

    private record GeminiEmbedResponse(
        [property: JsonPropertyName("embedding")]
        EmbeddingValues Embedding
    );

    private record EmbeddingValues(
        [property: JsonPropertyName("values")] float[] Values
    );
}