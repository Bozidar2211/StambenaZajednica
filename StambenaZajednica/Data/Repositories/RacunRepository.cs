using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Data;
using StambenaZajednica.Models;
using Microsoft.EntityFrameworkCore;

public class RacunRepository : IRacunRepository
{
    private readonly RepositoryDbContext _context;

    public RacunRepository(RepositoryDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Racun> GetRacuniZaStan(int stanId)
    {
        return _context.Racuni
            .Where(r => r.StanId == stanId)
            .Include(r => r.Uplate)
            .ToList();
    }

    public IEnumerable<Racun> GetNeplaceniRacuni(int stanId)
    {
        return _context.Racuni
            .Where(r => r.StanId == stanId && !r.JePlaceno)
            .Include(r => r.Uplate)
            .ToList();
    }

    public IEnumerable<Racun> GetRacuniZaZajednicu(int zajednicaId)
    {
        return _context.Racuni
            .Where(r => r.StambZajednicaId == zajednicaId)
            .Include(r => r.Uplate)   // da i stanar vidi uplate
            .ToList();
    }

    public IEnumerable<Racun> GetSviRacuni()
    {
        return _context.Racuni
            .Include(r => r.Stan)
            .Include(r => r.StambZajednica)
            .Include(r => r.Uplate)
            .ToList();
    }

    public Racun? GetRacunById(int id)
    {
        return _context.Racuni
            .Include(r => r.Uplate)
            .FirstOrDefault(r => r.Id == id);
    }

    public void AddRacun(Racun racun)
    {
        _context.Racuni.Add(racun);
        _context.SaveChanges();
    }

    public void UpdateRacun(Racun racun)
    {
        _context.Racuni.Update(racun);
        _context.SaveChanges();
    }

    public void DeleteRacun(int id)
    {
        var racun = _context.Racuni.Find(id);
        if (racun != null)
        {
            _context.Racuni.Remove(racun);
            _context.SaveChanges();
        }
    }
}
