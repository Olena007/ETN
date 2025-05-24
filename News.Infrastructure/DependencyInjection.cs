using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using News.BusinessLogic.Interfaces;

namespace News.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<INewsDbContext, NewsDbContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => { b.MigrationsAssembly("News"); });
        });
        return services;
    }
}