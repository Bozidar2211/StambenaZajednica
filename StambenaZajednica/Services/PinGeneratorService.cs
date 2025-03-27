using Microsoft.AspNetCore.Identity;
using StambenaZajednica.Models;

namespace StambenaZajednica.Services
{
    public class PinGeneratorService(UserManager<ApplicationUser> userManager)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        // Generišemo PIN za korisnika
        public async Task GeneratePinForUser(ApplicationUser user)
        {
            var pin = GeneratePin();
            if (user is Stanar stanar)
            {
                stanar.Pin = pin;
                await _userManager.UpdateAsync(stanar); // Ažuriramo korisnika u bazi
            }
        }

        // Privatna metoda za generisanje 6-cifrenog PIN-a
        private static string GeneratePin()
        {
            var random = new Random();
            var pin = random.Next(100000, 999999).ToString();  // Generišemo 6-cifreni PIN
            return pin;
        }
    }
}
