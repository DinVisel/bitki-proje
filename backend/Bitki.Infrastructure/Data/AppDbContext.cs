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

                        entity.Property(e => e.GenusId).HasColumnName("genusno");

                        entity.Ignore(e => e.FamilyName);
                        entity.Ignore(e => e.GenusName);

                        entity.Property(e => e.Description)
                        .HasColumnName("aciklama")
                        .IsRequired(false);

                        // Map boolean flags
                        entity.Property(e => e.IsMedicinal).HasColumnName("tibbi");
                        entity.Property(e => e.IsFood).HasColumnName("gida");
                        entity.Property(e => e.IsCultural).HasColumnName("kultur");
                        entity.Property(e => e.IsPoisonous).HasColumnName("zehir");
                        entity.Property(e => e.IsTurkishFlora).HasColumnName("tf");
                        entity.Property(e => e.IsIslandSpecies).HasColumnName("adalar");
                        entity.Property(e => e.ExistenceDoubtful).HasColumnName("varliksupheli");
                        entity.Property(e => e.NeedsRevision).HasColumnName("revizyon");
                        entity.Property(e => e.IsExtinct).HasColumnName("ex");
                        entity.Property(e => e.IncompleteIdentification).HasColumnName("eksikteshis");
                        entity.Property(e => e.ControlOk).HasColumnName("kontrolok");
                        entity.Property(e => e.PublicationOk).HasColumnName("yayinok");

                        // Map Ecology
                        entity.Property(e => e.Endemism).HasColumnName("endemizm");
                        entity.Property(e => e.EndemismDescription).HasColumnName("revizyonaciklama");
                        entity.Property(e => e.FirstFloweringTime).HasColumnName("ilkcicek");
                        entity.Property(e => e.LastFloweringTime).HasColumnName("soncicek");
                        entity.Property(e => e.Habitat).HasColumnName("hayatformu");
                        entity.Property(e => e.MinAltitude).HasColumnName("minyuseklik");
                        entity.Property(e => e.MaxAltitude).HasColumnName("maxyukseklik");
                        entity.Property(e => e.DistributionTurkey).HasColumnName("tdagilim");
                        entity.Property(e => e.DistributionWorld).HasColumnName("ddagilim");
                        entity.Property(e => e.Phytogeography).HasColumnName("davis");

                        // Map Taxonomy & Other
                        entity.Property(e => e.CommonNames).HasColumnName("sinonimler");
                        entity.Property(e => e.TaxonName).HasColumnName("species");
                        entity.Property(e => e.TaxonKind).HasColumnName("subspecies");
                  });
            }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                  return base.SaveChangesAsync(cancellationToken);
            }
      }
}
