using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Data;
using StambenaZajednica.Models;
using Microsoft.EntityFrameworkCore;

public class StanRepository : IStanRepository
{
    private readonly RepositoryDbContext _context;

    public StanRepository(RepositoryDbContext context)
    {
        _context = context;
    }

    public Stan? GetStanById(int id)
    {
        return _context.Stanovi
            .Include(s => s.Zajednica)
            .Include(s => s.Stanar)
            .FirstOrDefault(s => s.Id == id);
    }

    public IEnumerable<Stan> GetSviStanovi()
    {
        return _context.Stanovi
            .Include(s => s.Zajednica)
            .Include(s => s.Stanar)
            .ToList();
    }

    public IEnumerable<Stan> GetStanoviBezStanara()
    {
        return _context.Stanovi
            .Where(s => s.StanarId == null)
            .ToList();
    }

    public void DodeliStanKorisniku(int stanId, string userId)
    {
        var stan = _context.Stanovi.Find(stanId);
        if (stan != null)
        {
            stan.StanarId = userId;
            _context.SaveChanges();
        }
    }

    public void AddStan(Stan stan)
    {
        _context.Stanovi.Add(stan);
        _context.SaveChanges();
    }

    public void UpdateStan(Stan stan)
    {
        _context.Stanovi.Update(stan);
        _context.SaveChanges();
    }

    public void DeleteStan(int id)
    {
        var stan = _context.Stanovi.Find(id);
        if (stan != null)
        {
            _context.Stanovi.Remove(stan);
            _context.SaveChanges();
        }
    }
}
