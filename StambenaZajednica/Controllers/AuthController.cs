using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StambenaZajednica.Models;
using StambenaZajednica.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StambenaZajednica.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly PinGeneratorService _pinService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PinGeneratorService pinService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _pinService = pinService;
            _logger = logger;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Početak registracije za email: {Email}", model.Email);

                var user = new Stanar
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Stanar");
                    await _pinService.GeneratePinForUser(user); // Generišemo i dodeljujemo PIN
                    _logger.LogInformation("Uspešno kreiran korisnik sa email: {Email}", model.Email);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("Greška pri registraciji za email {Email}: {ErrorDescription}", model.Email, error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Početak prijave za email: {Email}", model.Email);

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Uspešna prijava za email: {Email}", model.Email);

                        // Provera da li je korisnik upravnik ili stanar
                        if (await _userManager.IsInRoleAsync(user, "Upravnik"))
                        {
                            // Ako je Upravnik, preusmeriti ga na početnu stranicu
                            return RedirectToAction("Index", "Home");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Stanar"))
                        {
                            // Ako je Stanar, preusmeriti ga na MyPin
                            return RedirectToAction("MyPin", "Auth");
                        }
                        else
                        {
                            // Ako nema odgovarajuću ulogu, prikazati grešku
                            return RedirectToAction("AccessDenied", "Account");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Neuspešna prijava za email: {Email}", model.Email);
                        ModelState.AddModelError(string.Empty, "Neuspešna prijava.");
                    }
                }
                else
                {
                    _logger.LogWarning("Neuspešna prijava za email: {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Neuspešna prijava.");
                }
            }

            return View(model);
        }
        [Authorize(Roles = "Stanar")]
        public async Task<IActionResult> MyPin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
#pragma warning disable CS8604 // Possible null reference argument.
            var stanar = await _userManager.FindByIdAsync(userId);
#pragma warning restore CS8604 // Possible null reference argument.

            if (stanar == null)
            {
                return NotFound();
            }

            return View(stanar);
        }
    }
}
