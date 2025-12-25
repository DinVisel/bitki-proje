using Microsoft.EntityFrameworkCore;
using Bitki.Core.Entities;

namespace Bitki.Infrastructure.Data
{
      public class AppDbContext : DbContext
      {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {
            }

            public DbSet<Plant> Plants { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  modelBuilder.Entity<Plant>(entity =>
                  {
                        entity.ToTable("bitki", "dbo");

                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Id)
                        .HasColumnName("bitkiid")
                        .ValueGeneratedOnAdd();

                        entity.Property(e => e.Name)
                        .HasColumnName("turkce")
                        .IsRequired(false);

                        entity.Property(e => e.LatinName)
                        .HasColumnName("bitki")
                        .IsRequired(false);

                        entity.Ignore(e => e.Family);
                        entity.Ignore(e => e.Habitat);

                        entity.Property(e => e.Description)
                        .HasColumnName("aciklama")
                        .IsRequired(false);

                        // Shadow properties for required boolean columns
                        entity.Property<bool>("tibbi").HasColumnName("tibbi");
                        entity.Property<bool>("gida").HasColumnName("gida");
                        entity.Property<bool>("kultur").HasColumnName("kultur");
                        entity.Property<bool>("zehir").HasColumnName("zehir");
                        entity.Property<bool>("tf").HasColumnName("tf");
                        entity.Property<bool>("adalar").HasColumnName("adalar");
                        entity.Property<bool>("varliksupheli").HasColumnName("varliksupheli");
                        entity.Property<bool>("revizyon").HasColumnName("revizyon");
                        entity.Property<bool>("ex").HasColumnName("ex");
                        entity.Property<bool>("eksikteshis").HasColumnName("eksikteshis");
                        entity.Property<bool>("kontrolok").HasColumnName("kontrolok");
                        entity.Property<bool>("yayinok").HasColumnName("yayinok");
                  });
            }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                  foreach (var entry in ChangeTracker.Entries<Plant>())
                  {
                        if (entry.State == EntityState.Added)
                        {
                              entry.Property("tibbi").CurrentValue = false;
                              entry.Property("gida").CurrentValue = false;
                              entry.Property("kultur").CurrentValue = false;
                              entry.Property("zehir").CurrentValue = false;
                              entry.Property("tf").CurrentValue = false;
                              entry.Property("adalar").CurrentValue = false;
                              entry.Property("varliksupheli").CurrentValue = false;
                              entry.Property("revizyon").CurrentValue = false;
                              entry.Property("ex").CurrentValue = false;
                              entry.Property("eksikteshis").CurrentValue = false;
                              entry.Property("kontrolok").CurrentValue = false;
                              entry.Property("yayinok").CurrentValue = false;
                        }
                  }
                  return base.SaveChangesAsync(cancellationToken);
            }
      }
}
