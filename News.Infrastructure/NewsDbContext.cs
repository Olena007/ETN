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
}