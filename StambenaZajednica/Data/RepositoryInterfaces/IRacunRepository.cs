using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IRacunRepository
    {
        IEnumerable<Racun> GetRacuniZaStan(int stanId);
        IEnumerable<Racun> GetNeplaceniRacuni(int stanId);
        IEnumerable<Racun> GetRacuniZaZajednicu(int zajednicaId);
        IEnumerable<Racun> GetSviRacuni();
        Racun? GetRacunById(int id);
        void AddRacun(Racun racun);
        void UpdateRacun(Racun racun);
        void DeleteRacun(int id);
    }
}
