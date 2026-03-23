using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using News.BusinessLogic.Embeddings;
using News.BusinessLogic.Interfaces;
using News.BusinessLogic.Recommendations;
using News.Enums;

namespace News.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }

    public static IServiceCollection AddBusinessLogic(
        this IServiceCollection services,
        EmbeddingProvider provider,
        string sentenceTransformersUrl = "",
        string geminiApiKey = "")
    {
        switch (provider)
        {
            case EmbeddingProvider.SentenceTransformers:
                services.AddHttpClient<IEmbeddingService, SentenceTransformerEmbedding>(c =>
                {
                    c.BaseAddress = new Uri(sentenceTransformersUrl);
                    c.Timeout = TimeSpan.FromSeconds(30);
                });
                services.AddScoped<IRecommendationService, SentenceTransformerRecommendation>();
                break;

            case EmbeddingProvider.Gemini:
                services.AddTransient<IEmbeddingService>(_ =>
                    new GeminiEmbedding(new HttpClient(), geminiApiKey));
                services.AddScoped<IRecommendationService, GeminiRecommendation>();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
        }

        return services;
    }
}