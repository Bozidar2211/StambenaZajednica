using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data.Repositories
{
    public class FinansijeRepository : IFinansijeRepository
    {
        private readonly RepositoryDbContext _context;

        public FinansijeRepository(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Finansije>> GetAllAsync()
        {
            return await _context.Finansije.ToListAsync();
        }
        public async Task<IEnumerable<Finansije>> GetAllForHousingCommunityAsync(int stambenaZajednicaId)
        {
            return await _context.Finansije
                .Where(f => f.StambenaZajednicaId == stambenaZajednicaId)
                .ToListAsync();
        }


        public async Task<Finansije> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Finansije.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task AddAsync(Finansije finansije)
        {
            await _context.Finansije.AddAsync(finansije);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Finansije finansije)
        {
            _context.Finansije.Update(finansije);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var finansije = await GetByIdAsync(id);
            if (finansije != null)
            {
                _context.Finansije.Remove(finansije);
                await _context.SaveChangesAsync();
            }
        }
    }
}
