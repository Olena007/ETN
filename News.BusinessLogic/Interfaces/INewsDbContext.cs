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
        DbSet<Car> Cars { get; }
        DbSet<User> Users { get; }
        DbSet<Entities.View> Views { get; }
        DbSet<Entities.News> News { get; }
        DbSet<Booking> Bookings { get; }
        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
