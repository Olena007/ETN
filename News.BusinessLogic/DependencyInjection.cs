using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using News.BusinessLogic.Embedding;
using News.BusinessLogic.Interfaces;
using News.Enums;

namespace News.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            return services;
        }
        
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, EmbeddingProvider provider, string sentenceTransformersUrl = "", string geminiApiKey = "") {
            switch (provider)
            {
                case EmbeddingProvider.SentenceTransformers:
                    services.AddHttpClient<IEmbeddingService, SentenceTransformerEmbeddingService>(client =>
                    {
                        client.BaseAddress = new Uri(sentenceTransformersUrl);
                        client.Timeout = TimeSpan.FromSeconds(30);
                    });
                    break;

                case EmbeddingProvider.Gemini:
                    services.AddTransient<IEmbeddingService>(sp =>
                    {
                        var httpFactory = sp.GetRequiredService<IHttpClientFactory>();
                        var http = httpFactory.CreateClient();
                        return new GeminiEmbeddingService(http, geminiApiKey);
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
            }

            services.AddSingleton(typeof(EmbeddingProvider), provider); 
            services.AddScoped<IRecommendationService, RecommendationService>();
            return services;
        }
    }
}
