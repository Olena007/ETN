using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.Infrastructure;

public class NewsDbContext : DbContext, INewsDbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> opts) : base(opts)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<View> Views => Set<View>();
    public DbSet<Entities.News> News => Set<Entities.News>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Source> Sources => Set<Source>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new ValueConverter<Dictionary<string, List<string>>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<Dictionary<string, List<string>>>(v, (JsonSerializerOptions)null));
    }
}