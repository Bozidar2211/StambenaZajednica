using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IStambenaZajednicaRepository
    {
        IEnumerable<StambZajednica> GetSveZajednice();
        StambZajednica? GetZajednicaById(int id);
        void AddZajednica(StambZajednica zajednica);
        void UpdateZajednica(StambZajednica zajednica);
        void DeleteZajednica(int id);
        IEnumerable<Stan> GetStanoviZaZajednicu(int zajednicaId);
    }
}