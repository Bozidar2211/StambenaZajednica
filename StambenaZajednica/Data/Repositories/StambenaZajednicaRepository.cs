using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data.Repositories
{
        public class StambenaZajednicaRepository : IStambenaZajednicaRepository
        {
            private readonly RepositoryDbContext _context;

            public StambenaZajednicaRepository(RepositoryDbContext context)
            {
                _context = context;
            }

            public async Task<List<StambZajednica>> GetAllAsync()
            {
                return await _context.StambeneZajednice.ToListAsync();
            }

            public async Task<StambZajednica> GetByIdAsync(int id)
            {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.StambeneZajednice.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

            public async Task AddAsync(StambZajednica stambenaZajednica)
            {
                await _context.StambeneZajednice.AddAsync(stambenaZajednica);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(StambZajednica stambenaZajednica)
            {
                _context.StambeneZajednice.Update(stambenaZajednica);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var stambenaZajednica = await GetByIdAsync(id);
                if (stambenaZajednica != null)
                {
                    _context.StambeneZajednice.Remove(stambenaZajednica);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
