using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Controllers
{
    [Authorize]
    public class UplataController : Controller
    {
        private readonly IRacunRepository _racunRepo;
        private readonly IUplataRepository _uplataRepo;
        private readonly IUserRepository _userRepo;

        public UplataController(IRacunRepository racunRepo,
                                IUplataRepository uplataRepo,
                                IUserRepository userRepo)
        {
            _racunRepo = racunRepo;
            _uplataRepo = uplataRepo;
            _userRepo = userRepo;
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Index()
        {
            var sveUplate = _uplataRepo.GetSveUplate();
            return View(sveUplate);
        }

        [Authorize(Roles = "Upravnik")]
        public IActionResult Create()
        {
            ViewBag.Racuni = new SelectList(_racunRepo.GetSviRacuni(), "Id", "Opis");
            ViewBag.Stanari = new SelectList(_userRepo.GetSviKorisnici(), "Id", "UserName");
            return View();
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public IActionResult Create(Uplata uplata)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Racuni = new SelectList(_racunRepo.GetSviRacuni(), "Id", "Opis");
                ViewBag.Stanari = new SelectList(_userRepo.GetSviKorisnici(), "Id", "UserName");
                return View(uplata);
            }

            uplata.DatumUplate = DateTime.Now;
            _uplataRepo.AddUplata(uplata);

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Stanar")]
        public async Task<IActionResult> MojeUplate()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return RedirectToAction("Login", "Account");

            var korisnik = await _userRepo.GetKorisnikByUsernameAsync(userName);
            if (korisnik == null)
                return RedirectToAction("Login", "Account");

            var mojeUplate = _uplataRepo.GetSveUplate()
                .Where(u => u.StanarId == korisnik.Id)
                .ToList();

            return View(mojeUplate);
        }
        [Authorize(Roles = "Stanar")]
        public IActionResult Plati(int id)
        {
            var racun = _racunRepo.GetRacunById(id);
            if (racun == null) return NotFound();

            return View(racun);
        }
        [Authorize(Roles = "Stanar")]
        [HttpPost]
        public async Task<IActionResult> Plati(int id, decimal iznos)
        {
            var racun = _racunRepo.GetRacunById(id);
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            var korisnik = await _userRepo.GetKorisnikByUsernameAsync(username);

            if (racun == null || korisnik == null) return NotFound();

            var uplata = new Uplata
            {
                RacunId = racun.Id,
                StanarId = korisnik.Id,
                Iznos = iznos,
                DatumUplate = DateTime.Now
            };

            _uplataRepo.AddUplata(uplata);

            var ukupnoUplaceno = _uplataRepo.GetSveUplate()
                .Where(u => u.RacunId == racun.Id)
                .Sum(u => u.Iznos);

            racun.JePlaceno = ukupnoUplaceno >= racun.Iznos;
            _racunRepo.UpdateRacun(racun);

            return RedirectToAction("Index", "Finansije");
        }

    }
}
