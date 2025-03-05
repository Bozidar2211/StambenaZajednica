using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces

{
    public interface IStambenaZajednicaRepository
    {
        Task<List<StambZajednica>> GetAllAsync();
        Task<StambZajednica> GetByIdAsync(int id);
        Task AddAsync(StambZajednica stambenaZajednica);
        Task UpdateAsync(StambZajednica stambenaZajednica);
        Task DeleteAsync(int id);
    }
}
