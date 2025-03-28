using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IFinansijeRepository
    {
        Task<List<Finansije>> GetAllAsync();
        Task<Finansije> GetByIdAsync(int id);
        // Dohvatanje finansija na osnovu stambene zajednice
        Task<IEnumerable<Finansije>> GetAllForHousingCommunityAsync(int stambenaZajednicaId);
        Task AddAsync(Finansije finansije);
        Task UpdateAsync(Finansije finansije);
        Task DeleteAsync(int id);
    }
}
