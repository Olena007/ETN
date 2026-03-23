using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using News.BusinessLogic.Interfaces;

namespace News.BusinessLogic.Embeddings;

public class OpenAiEmbedding(HttpClient http, string apiKey) : IEmbeddingService
{
    private const string Model = "text-embedding-3-small"; // 1536 dims

    public async Task<float[]> GenerateAsync(string text, CancellationToken ct = default)
    {
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new { input = text, model = Model };

        var response = await http.PostAsJsonAsync("https://api.openai.com/v1/embeddings", body, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"OpenAI API error {response.StatusCode}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<OpenAiEmbedResponse>(ct);
        return result!.Data[0].Embedding;
    }

    public async Task<float[][]> GenerateBatchAsync(IEnumerable<string> texts, CancellationToken ct = default)
    {
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new { input = texts.ToArray(), model = Model };

        var response = await http.PostAsJsonAsync("https://api.openai.com/v1/embeddings", body, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"OpenAI API error {response.StatusCode}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<OpenAiEmbedResponse>(ct);
        return result!.Data.OrderBy(d => d.Index).Select(d => d.Embedding).ToArray();
    }

    private record OpenAiEmbedResponse(
        [property: JsonPropertyName("data")] List<EmbeddingData> Data
    );

    private record EmbeddingData(
        [property: JsonPropertyName("index")] int Index,
        [property: JsonPropertyName("embedding")]
        float[] Embedding
    );
}