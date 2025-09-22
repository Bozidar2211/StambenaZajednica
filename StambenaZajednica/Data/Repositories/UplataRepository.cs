using StambenaZajednica.Data;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;
using Microsoft.EntityFrameworkCore;

public class UplataRepository : IUplataRepository
{
    private readonly RepositoryDbContext _context;

    public UplataRepository(RepositoryDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Uplata> GetUplateZaRacun(int racunId)
    {
        return _context.Uplate
            .Where(u => u.RacunId == racunId)
            .ToList();
    }

    public IEnumerable<Uplata> GetSveUplate()
    {
        return _context.Uplate
            .Include(u => u.Racun)
            .Include(u => u.Stanar)
            .ToList();
    }

    public Uplata? GetUplataById(int id)
    {
        return _context.Uplate.FirstOrDefault(u => u.Id == id);
    }

    public void AddUplata(Uplata uplata)
    {
        _context.Uplate.Add(uplata);
        _context.SaveChanges();
    }

    public void DeleteUplata(int id)
    {
        var uplata = GetUplataById(id);
        if (uplata != null)
        {
            _context.Uplate.Remove(uplata);
            _context.SaveChanges();
        }
    }
}
