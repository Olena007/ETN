using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using News.BusinessLogic.Embedding;
using News.BusinessLogic.Interfaces;

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
        
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, string embeddingServiceUrl)
        {
            services.AddHttpClient<IEmbeddingService, SentenceTransformerEmbeddingService>(client =>
            {
                client.BaseAddress = new Uri(embeddingServiceUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddScoped<IRecommendationService, RecommendationService>();

            return services;
        }
    }
}
