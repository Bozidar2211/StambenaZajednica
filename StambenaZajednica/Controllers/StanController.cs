using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;
using StambenaZajednica.Helpers;

namespace StambenaZajednica.Controllers
{
    [Authorize]
    public class StanController : Controller
    {
        private readonly IStanRepository _stanRepo;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStambenaZajednicaRepository _zajednicaRepo;

        public StanController(
            IStanRepository stanRepo,
            IUserRepository userRepo,
            UserManager<ApplicationUser> userManager,
            IStambenaZajednicaRepository zajednicaRepo)
        {
            _stanRepo = stanRepo;
            _userRepo = userRepo;
            _userManager = userManager;
            _zajednicaRepo = zajednicaRepo;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity?.Name;

            var user = await _userManager.Users
                .Include(u => u.Stan)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) return RedirectToAction("Login", "Account");

            if (User.IsInRole("Upravnik"))
                return View(_stanRepo.GetSviStanovi());

            if (user.StanId != null)
            {
                var mojStan = _stanRepo.GetStanById(user.StanId.Value);
                if (mojStan != null)
                    return View(new List<Stan> { mojStan });
            }

            return View(new List<Stan>());
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Create()
        {
            ViewBag.Zajednice = new SelectList(
                _zajednicaRepo.GetSveZajednice(), "Id", "Naziv");
            return View();
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public IActionResult Create(Stan stan)
        {
            // Ručna validacija
            if (string.IsNullOrWhiteSpace(stan.BrojStana))
                ModelState.AddModelError("BrojStana", "Broj stana je obavezan.");

            if (stan.Kvadratura <= 0)
                ModelState.AddModelError("Kvadratura", "Kvadratura mora biti veća od 0.");

            if (stan.ZajednicaId <= 0)
                ModelState.AddModelError("ZajednicaId", "Morate odabrati zajednicu.");

            if (!ModelState.IsValid)
            {
                ViewBag.Zajednice = new SelectList(
                    _zajednicaRepo.GetSveZajednice(), "Id", "Naziv");
                return View(stan);
            }

            _stanRepo.AddStan(stan);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Edit(int id)
        {
            var stan = _stanRepo.GetStanById(id);
            if (stan == null) return NotFound();

            ViewBag.Zajednice = new SelectList(
                _zajednicaRepo.GetSveZajednice(), "Id", "Naziv", stan.ZajednicaId);
            return View(stan);
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public IActionResult Edit(Stan stan)
        {
            // Ručna validacija
            if (string.IsNullOrWhiteSpace(stan.BrojStana))
                ModelState.AddModelError("BrojStana", "Broj stana je obavezan.");

            if (stan.Kvadratura <= 0)
                ModelState.AddModelError("Kvadratura", "Kvadratura mora biti veća od 0.");

            if (stan.ZajednicaId <= 0)
                ModelState.AddModelError("ZajednicaId", "Morate odabrati zajednicu.");

            if (!ModelState.IsValid)
            {
                ViewBag.Zajednice = new SelectList(
                    _zajednicaRepo.GetSveZajednice(), "Id", "Naziv", stan.ZajednicaId);
                return View(stan);
            }

            var postojeci = _stanRepo.GetStanById(stan.Id);
            if (postojeci == null) return NotFound();

            // Ažuriraj samo osnovna polja
            postojeci.BrojStana = stan.BrojStana;
            postojeci.Kvadratura = stan.Kvadratura;
            postojeci.ZajednicaId = stan.ZajednicaId;

            _stanRepo.UpdateStan(postojeci);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Delete(int id)
        {
            var stan = _stanRepo.GetStanById(id);
            if (stan == null) return NotFound();

            return View(stan);
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stan = _stanRepo.GetStanById(id);
            if (stan == null) return NotFound();

            // Odveži sve korisnike koji su vezani za ovaj stan
            var korisnici = _userManager.Users.Where(u => u.StanId == id).ToList();
            foreach (var k in korisnici)
            {
                k.StanId = null;
                await _userManager.UpdateAsync(k);
            }

            _stanRepo.DeleteStan(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Dodeli(int stanId)
        {
            var stan = _stanRepo.GetStanById(stanId);
            if (stan == null) return NotFound();

            ViewBag.StanId = stanId;
            return View(_userRepo.GetSviKorisniciBezStana());
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public async Task<IActionResult> DodeliStan(int stanId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                ModelState.AddModelError("", "Morate izabrati korisnika.");
                ViewBag.StanId = stanId;
                return View("Dodeli", _userRepo.GetSviKorisniciBezStana());
            }

            var stan = _stanRepo.GetStanById(stanId);
            var korisnik = _userRepo.GetKorisnikById(userId);

            if (stan == null || korisnik == null) return NotFound();

            stan.StanarId = userId;
            stan.Pin = PinHelper.GenerisiNoviPIN();
            _stanRepo.UpdateStan(stan);

            korisnik.StanId = stan.Id;
            await _userManager.UpdateAsync(korisnik);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Stanar")]
        public async Task<IActionResult> MojStan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.StanId == null) return RedirectToAction("Index", "Home");

            var stan = _stanRepo.GetStanById(user.StanId.Value);
            if (stan == null) return NotFound();

            return View(stan);
        }
    }
}
