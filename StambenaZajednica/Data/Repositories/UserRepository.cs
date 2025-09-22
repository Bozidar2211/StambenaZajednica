using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetSviKorisniciBezStana()
        {
            return _userManager.Users
                .Where(u => u.StanId == null)
                .ToList();
        }

        public ApplicationUser? GetKorisnikById(string id)
        {
            return _userManager.Users
                .FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<ApplicationUser> GetSviKorisnici()
        {
            return _userManager.Users.ToList();
        }
        public async Task<ApplicationUser?> GetKorisnikByUsernameAsync(string username)
        {
            return await _userManager.Users
                .Include(u => u.Stan)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

    }
}
