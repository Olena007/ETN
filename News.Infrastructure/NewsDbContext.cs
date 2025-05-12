using System.Text.Json;
using News.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<View> Views => Set<View>();
        public DbSet<Entities.News> News => Set<Entities.News>();

        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<Dictionary<string, List<string>>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, List<string>>>(v, (JsonSerializerOptions)null));
        }
    }
}
