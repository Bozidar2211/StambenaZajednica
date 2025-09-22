using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data
{
    public class RepositoryDbContext : IdentityDbContext<ApplicationUser>
    {
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<StambZajednica> StambeneZajednice { get; set; }
        public DbSet<Stan> Stanovi { get; set; }
        public DbSet<Racun> Racuni { get; set; }
        public DbSet<Uplata> Uplate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // StambenaZajednica → Stan (1:N)
            modelBuilder.Entity<Stan>()
                .HasOne(s => s.Zajednica)
                .WithMany(z => z.Stanovi)
                .HasForeignKey(s => s.ZajednicaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Stan → ApplicationUser (1:1, opcionalno)
            modelBuilder.Entity<Stan>()
                .HasOne(s => s.Stanar)
                .WithMany()
                .HasForeignKey(s => s.StanarId)
                .OnDelete(DeleteBehavior.SetNull);

            // Stan → Racun (1:N)
            modelBuilder.Entity<Racun>()
                .HasOne(r => r.Stan)
                .WithMany(s => s.Racuni)
                .HasForeignKey(r => r.StanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Racun → Uplata (1:N)
            modelBuilder.Entity<Uplata>()
                .HasOne(u => u.Racun)
                .WithMany(r => r.Uplate)
                .HasForeignKey(u => u.RacunId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal preciznost za finansije
            modelBuilder.Entity<Racun>()
                .Property(r => r.Iznos)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Uplata>()
                .Property(u => u.Iznos)
                .HasColumnType("decimal(18,2)");
        }
    }
}
