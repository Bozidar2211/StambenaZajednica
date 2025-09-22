using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IStanRepository
    {
        Stan? GetStanById(int id);
        IEnumerable<Stan> GetSviStanovi();
        IEnumerable<Stan> GetStanoviBezStanara();
        void DodeliStanKorisniku(int stanId, string userId);
        void AddStan(Stan stan);
        void UpdateStan(Stan stan);
        void DeleteStan(int id);
    }
}
