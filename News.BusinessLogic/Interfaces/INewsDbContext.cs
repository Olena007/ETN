using System.Threading;
using System.Threading.Tasks;
using News.Entities;
using Microsoft.EntityFrameworkCore;

namespace News.BusinessLogic.Interfaces
{
    public interface INewsDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Article> Articles { get; }
        DbSet<ArticleUnit> ArticleUnits { get; }
        DbSet<Category> Categories { get; }
        DbSet<ThreadInfo> ThreadInfos { get; }
        DbSet<ArticleEmbedding> ArticleEmbeddings { get; }
        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
