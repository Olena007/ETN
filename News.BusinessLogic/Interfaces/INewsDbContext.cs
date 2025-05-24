using Microsoft.EntityFrameworkCore;
using News.Entities;

namespace News.BusinessLogic.Interfaces;

public interface INewsDbContext
{
    DbSet<User> Users { get; }
    DbSet<Entities.View> Views { get; }
    DbSet<Entities.News> News { get; }
    DbSet<Author> Authors { get; }
    DbSet<Category> Categories { get; }
    DbSet<Video> Videos { get; }
    DbSet<Source> Sources { get; }
    Task<int> SaveChangesAsync(CancellationToken token);
}