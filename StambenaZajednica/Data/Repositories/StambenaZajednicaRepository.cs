using StambenaZajednica.Data;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;
using Microsoft.EntityFrameworkCore;

public class StambenaZajednicaRepository : IStambenaZajednicaRepository
{
    private readonly RepositoryDbContext _context;

    public StambenaZajednicaRepository(RepositoryDbContext context)
    {
        _context = context;
    }

    public IEnumerable<StambZajednica> GetSveZajednice()
    {
        return _context.StambeneZajednice
            .Include(z => z.Stanovi)
            .ToList();
    }

    public StambZajednica? GetZajednicaById(int id)
    {
        return _context.StambeneZajednice
            .Include(z => z.Stanovi)
            .FirstOrDefault(z => z.Id == id);
    }

    public void AddZajednica(StambZajednica zajednica)
    {
        _context.StambeneZajednice.Add(zajednica);
        _context.SaveChanges();
    }

    public void UpdateZajednica(StambZajednica zajednica)
    {
        _context.StambeneZajednice.Update(zajednica);
        _context.SaveChanges();
    }

    public void DeleteZajednica(int id)
    {
        var zajednica = _context.StambeneZajednice.Find(id);
        if (zajednica != null)
        {
            _context.StambeneZajednice.Remove(zajednica);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Stan> GetStanoviZaZajednicu(int zajednicaId)
    {
        return _context.Stanovi
            .Where(s => s.ZajednicaId == zajednicaId)
            .Include(s => s.Stanar)
            .ToList();
    }
}
