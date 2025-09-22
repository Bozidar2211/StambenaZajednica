using Microsoft.AspNetCore.Identity;
using StambenaZajednica.Models;

namespace StambenaZajednica.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }

        public int? StanId { get; set; }
        public Stan? Stan { get; set; }
        public ICollection<Uplata>? Uplate { get; set; }



        public int PinAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
    }
}