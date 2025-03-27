using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data
{
    public class RepositoryDbContext : IdentityDbContext<ApplicationUser> 
    {
        public DbSet<StambZajednica> StambeneZajednice { get; set; }
        public DbSet<Stan> Stanovi { get; set; }
        public DbSet<Finansije> Finansije { get; set; }

        // Konstruktor koji prima DbContextOptions i prosleđuje ga bazi klasi
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
            : base(options) // Pozivamo bazni konstruktor IdentityDbContext
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Poziv baze za Identity

            // Definisanje primarnih ključeva
            modelBuilder.Entity<StambZajednica>()
                .HasKey(sz => sz.Id);

            modelBuilder.Entity<Stan>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Finansije>()
                .HasKey(f => f.Id);

            // Relacija: StambenaZajednica -> Stan (jedan-na-više)
            modelBuilder.Entity<Stan>()
                .HasOne(s => s.StambenaZajednica)
                .WithMany(sz => sz.Stanovi)
                .HasForeignKey(s => s.StambenaZajednicaId);

            // Relacija: StambenaZajednica -> Finansija (jedan-na-više)
            modelBuilder.Entity<Finansije>()
                .HasOne(f => f.StambenaZajednica)
                .WithMany(sz => sz.Finansije)
                .HasForeignKey(f => f.StambenaZajednicaId);

            // Relacija: Stan -> Stanar (jedan-na-jedan, opcionalno)
            modelBuilder.Entity<Stan>()
                .HasOne(s => s.Stanar)    // Veza sa Stanarom
                .WithMany()               // Nema potrebe za definisanjem obrnute veze
                .HasForeignKey(s => s.StanarId)  // Strani ključ
                .OnDelete(DeleteBehavior.SetNull); // Ako je Stanar obrisan, setuj StanarId na null

            // Definisanje preciznosti za decimalne vrednosti
            modelBuilder.Entity<Finansije>()
                .Property(f => f.IznosDuga)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Finansije>()
                .Property(f => f.IznosUplate)
                .HasColumnType("decimal(18,2)");
        }
    }
}
