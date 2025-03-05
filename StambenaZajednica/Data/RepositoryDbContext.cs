using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data
{
    public class RepositoryDbContext : DbContext
    {
        public DbSet<StambZajednica> StambeneZajednice { get; set; }
        public DbSet<Stan> Stanovi { get; set; }
        public DbSet<Finansije> Finansije { get; set; }

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
            : base(options)
        {
        }
    }
}
