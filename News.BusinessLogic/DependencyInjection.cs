using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using News.BusinessLogic.Embedding;
using News.BusinessLogic.Interfaces;
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
                services.AddHttpClient<IEmbeddingService, SentenceTransformerEmbeddingService>(c =>
                {
                    c.BaseAddress = new Uri(sentenceTransformersUrl);
                    c.Timeout = TimeSpan.FromSeconds(30);
                });
                services.AddScoped<IRecommendationService, SentenceTransformerRecommendationService>();
                break;

            case EmbeddingProvider.Gemini:
                services.AddTransient<IEmbeddingService>(_ =>
                    new GeminiEmbeddingService(new HttpClient(), geminiApiKey));
                services.AddScoped<IRecommendationService, GeminiRecommendationService>();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
        }

        return services;
    }
}