using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data.Repositories
{
        public class StanRepository : IStanRepository
        {
            private readonly RepositoryDbContext _context;

            public StanRepository(RepositoryDbContext context)
            {
                _context = context;
            }

            public async Task<List<Stan>> GetAllAsync()
            {
                return await _context.Stanovi.ToListAsync();
            }

            public async Task<Stan> GetByIdAsync(int id)
            {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Stanovi.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

            public async Task AddAsync(Stan stan)
            {
                await _context.Stanovi.AddAsync(stan);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Stan stan)
            {
                _context.Stanovi.Update(stan);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var stan = await GetByIdAsync(id);
                if (stan != null)
                {
                    _context.Stanovi.Remove(stan);
                    await _context.SaveChangesAsync();
                }
            }
        }
}
