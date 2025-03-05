using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IStanRepository
    {
        Task<List<Stan>> GetAllAsync();
        Task<Stan> GetByIdAsync(int id);
        Task AddAsync(Stan stan);
        Task UpdateAsync(Stan stan);
        Task DeleteAsync(int id);
    }
}
