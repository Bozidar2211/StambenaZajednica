using StambenaZajednica.Models;

namespace StambenaZajednica.Data.RepositoryInterfaces
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> GetSviKorisniciBezStana();
        IEnumerable<ApplicationUser> GetSviKorisnici();
        Task<ApplicationUser?> GetKorisnikByUsernameAsync(string username);


        ApplicationUser? GetKorisnikById(string id);
    }
}
