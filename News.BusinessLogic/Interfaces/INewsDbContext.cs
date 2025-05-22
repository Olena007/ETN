using News.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace News.BusinessLogic.Interfaces
{
    public interface INewsDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Entities.View> Views { get; }
        DbSet<Entities.News> News { get; }
        DbSet<Author> Authors { get; }
        DbSet<Category>  Categories { get; }
        DbSet<Video>  Videos { get; }
        DbSet<Source>  Sources { get; }
        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
