using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;
using StambenaZajednica.Helpers; 

namespace StambenaZajednica.Controllers
{
    [Authorize]
    public class FinansijeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRacunRepository _racunRepo;
        private readonly IStanRepository _stanRepo;
        private readonly IStambenaZajednicaRepository _zajednicaRepo;

        public FinansijeController(UserManager<ApplicationUser> userManager,
                                   IRacunRepository racunRepo,
                                   IStanRepository stanRepo,
                                   IStambenaZajednicaRepository zajednicaRepo)
        {
            _userManager = userManager;
            _racunRepo = racunRepo;
            _stanRepo = stanRepo;
            _zajednicaRepo = zajednicaRepo;
        }

        // GET: forma za unos PIN-a ili pregled svih računa za upravnika
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Upravnik"))
            {
                var sviRacuni = _racunRepo.GetSviRacuni();
                return View("Finansije", sviRacuni);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user?.StanId == null) return RedirectToAction("Index", "Home");

            ViewBag.StanId = user.StanId;
            return View(); // prikazuje formu za unos PIN-a
        }

        // POST: provera PIN-a
        [HttpPost]
        public async Task<IActionResult> Index(string pin)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.StanId == null) return RedirectToAction("Index", "Home");

            var stan = _stanRepo.GetStanById(user.StanId.Value);
            if (stan == null) return RedirectToAction("Index", "Home");

            if (stan.FinansijeZakljucane)
            {
                ModelState.AddModelError("", "Pristup finansijama je zaključan. Obratite se upravniku.");
                return View();
            }

            // Validacija PIN-a
            if (string.IsNullOrWhiteSpace(pin) || pin.Length != 6 || stan.Pin != pin)
            {
                stan.PinAttempts++;
                if (stan.PinAttempts >= 3)
                {
                    stan.FinansijeZakljucane = true;
                }
                _stanRepo.UpdateStan(stan);
                ModelState.AddModelError("", "Pogrešan PIN.");
                return View();
            }

            // PIN je tačan
            stan.PinAttempts = 0;
            _stanRepo.UpdateStan(stan);

            var racuniStana = _racunRepo.GetRacuniZaStan(stan.Id);
            var racuniZajednice = _racunRepo.GetRacuniZaZajednicu(stan.ZajednicaId);

            var sviRacuni = racuniStana
                .UnionBy(racuniZajednice, r => r.Id)
                .ToList();

            return View("Finansije", sviRacuni);
        }

        // 📌 Administracija za upravnika
        [Authorize(Roles = "Upravnik")]
        public IActionResult Administracija()
        {
            var sviRacuni = _racunRepo.GetSviRacuni();
            return View(sviRacuni);
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Create()
        {
            ViewBag.Stanovi = new SelectList(_stanRepo.GetSviStanovi(), "Id", "BrojStana");
            ViewBag.Zajednice = new SelectList(_zajednicaRepo.GetSveZajednice(), "Id", "Naziv");
            return View();
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public IActionResult Create(Racun racun)
        {
            //greske
            if (ModelState["Iznos"]?.Errors.Count > 0)
                ModelState.AddModelError("Iznos", "Iznos mora biti veći od 0.");

            if (ModelState["DatumDospeca"]?.Errors.Count > 0)
                ModelState.AddModelError("DatumDospeca", "Morate uneti datum dospeća.");

            if (ModelState["Tip"]?.Errors.Count > 0)
                ModelState.AddModelError("Tip", "Morate odabrati tip troška.");

            if (racun.StambZajednicaId <= 0)
                ModelState.AddModelError("StambZajednicaId", "Morate odabrati zajednicu.");

            if (!ModelState.IsValid)
            {
                ViewBag.Stanovi = new SelectList(_stanRepo.GetSviStanovi(), "Id", "BrojStana");
                ViewBag.Zajednice = new SelectList(_zajednicaRepo.GetSveZajednice(), "Id", "Naziv");
                return View(racun);
            }

            racun.Datum = DateTime.Now;
            _racunRepo.AddRacun(racun);
            return RedirectToAction("Administracija");
        }


        [Authorize(Roles = "Upravnik")]
        public IActionResult OznaciPlaceno(int id)
        {
            var racun = _racunRepo.GetRacunById(id);
            if (racun == null) return NotFound();

            if (!racun.JePlaceno)
            {
                racun.JePlaceno = true;
                _racunRepo.UpdateRacun(racun);
            }

            return RedirectToAction("Administracija");
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Delete(int id)
        {
            var racun = _racunRepo.GetRacunById(id);
            if (racun == null) return NotFound();

            return View(racun);
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _racunRepo.DeleteRacun(id);
            return RedirectToAction("Administracija", "Finansije");
        }

        // 📌 Reset PIN-a od strane upravnika
        [Authorize(Roles = "Upravnik")]
        public IActionResult ResetujPin(int stanId)
        {
            var stan = _stanRepo.GetStanById(stanId);
            if (stan == null) return NotFound();

            stan.FinansijeZakljucane = false;
            stan.PinAttempts = 0;
            stan.DatumOtkljucavanjaFinansija = DateTime.Now;
            stan.ObavestenONovomPINu = false;
            stan.Pin = PinHelper.GenerisiNoviPIN(); // helper

            _stanRepo.UpdateStan(stan);
            return RedirectToAction("Index","Stan");
        }
    }
}
