using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IFinansijeRepository
    {
        Task<List<Finansije>> GetAllAsync();
        Task<Finansije> GetByIdAsync(int id);
        Task AddAsync(Finansije finansije);
        Task UpdateAsync(Finansije finansije);
        Task DeleteAsync(int id);
    }
}
