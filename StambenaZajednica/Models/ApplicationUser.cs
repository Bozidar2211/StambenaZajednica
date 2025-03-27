using Microsoft.AspNetCore.Identity;

namespace StambenaZajednica.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }  // Ime korisnika
        public string? Prezime { get; set; }  // Prezime korisnika
        public string? Gmail { get; set; }  // Email korisnika

    }
}
