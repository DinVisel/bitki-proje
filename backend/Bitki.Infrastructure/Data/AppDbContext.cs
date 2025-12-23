using Microsoft.EntityFrameworkCore;

namespace Bitki.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bitki.Core.Entities.Plant> Plants { get; set; }
    }
}
