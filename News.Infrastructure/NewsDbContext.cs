using News.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using News.Entities;

namespace News.Infrastructure
{
    public class NewsDbContext: DbContext, INewsDbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> opts) : base(opts)
        {
        }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<User> Users => Set<User>();

        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
