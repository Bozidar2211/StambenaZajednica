using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IUplataRepository
    {
        IEnumerable<Uplata> GetUplateZaRacun(int racunId);
        IEnumerable<Uplata> GetSveUplate(); // NOVO
        Uplata? GetUplataById(int id);
        void AddUplata(Uplata uplata);
        void DeleteUplata(int id);
    }
}
