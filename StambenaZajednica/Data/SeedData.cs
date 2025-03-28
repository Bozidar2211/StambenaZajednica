using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StambenaZajednica.Models;

namespace StambenaZajednica.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roleNameUpravnik = "Upravnik";
            var roleNameStanar = "Stanar";

            // Kreiraj ulogu Upravnik ako ne postoji
            var roleUpravnik = await roleManager.FindByNameAsync(roleNameUpravnik);
            if (roleUpravnik == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleNameUpravnik));
            }

            // Kreiraj ulogu Stanar ako ne postoji
            var roleStanar = await roleManager.FindByNameAsync(roleNameStanar);
            if (roleStanar == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleNameStanar));
            }

            // Dodaj Upravnik korisnika ako ne postoji
            var upravnik = await userManager.FindByEmailAsync("upravnik@admin.com");
            if (upravnik == null)
            {
                upravnik = new ApplicationUser
                {
                    UserName = "upravnik@admin.com",
                    Email = "upravnik@admin.com"
                };
                var result = await userManager.CreateAsync(upravnik, "HardK0d0vanaSifra!"); // Postavi hardkodovanu šifru za Upravnika
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(upravnik, roleNameUpravnik);
                }
            }
        }
    }
}
