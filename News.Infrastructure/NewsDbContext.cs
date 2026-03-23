using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News.BusinessLogic.Interfaces;
using News.Entities;

namespace News.Infrastructure;

public class NewsDbContext(DbContextOptions<NewsDbContext> opts) : DbContext(opts), INewsDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleUnit> ArticleUnits => Set<ArticleUnit>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ThreadInfo> ThreadInfos => Set<ThreadInfo>();
    public DbSet<ArticleEmbedding> ArticleEmbeddings => Set<ArticleEmbedding>();
    public DbSet<ArticleEmbeddingGemini> ArticleEmbeddingsGemini => Set<ArticleEmbeddingGemini>();
    public DbSet<ArticleEmbeddingOpenAi> ArticleEmbeddingsOpenAi => Set<ArticleEmbeddingOpenAi>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.HasPostgresExtension("vector");
    
        modelBuilder.Entity<ArticleEmbedding>(e => {
            e.ToTable("ArticleEmbeddings");
            e.Property(x => x.Vector)
                .HasColumnType("vector(384)"); 
        });

        modelBuilder.Entity<ArticleEmbeddingGemini>(e => {
            e.ToTable("ArticleEmbeddingsGemini"); 
            e.Property(x => x.Vector)
                .HasColumnType("vector(3072)");
        });
        
        modelBuilder.Entity<ArticleEmbeddingOpenAi>(e => {
            e.ToTable("ArticleEmbeddingsOpenAi");
            e.Property(x => x.Vector).HasColumnType("vector(1536)");
        });
    }
}