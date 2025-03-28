using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Data;
using StambenaZajednica.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StambenaZajednica.Controllers
{
    [Authorize] // Samo prijavljeni korisnici mogu pristupiti
    public class FinansijeController : Controller
    {
        private readonly IFinansijeRepository _finansijeRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RepositoryDbContext _context;

        public FinansijeController(IFinansijeRepository finansijeRepo, UserManager<ApplicationUser> userManager, RepositoryDbContext context)
        {
            _finansijeRepo = finansijeRepo;
            _userManager = userManager;
            _context = context;
        }

        // 📌 Prikaz svih finansija - različit pristup za upravnika i stanara
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Upravnik"))
            {
                var sveFinansije = await _finansijeRepo.GetAllAsync();
                return View(sveFinansije);
            }

            // Stanar - dohvatamo njegovu stambenu zajednicu
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var stan = await _context.Stanovi.FirstOrDefaultAsync(s => s.StanarId == user.Id);

            if (stan == null)
            {
                return NotFound("Nema povezan stan.");
            }

            var finansije = stan != null
            ? await _finansijeRepo.GetAllForHousingCommunityAsync(stan.StambenaZajednicaId)
            : new List<Finansije>();

            return View(finansije);
        }

        // 📌 GET: Prikaz forme za kreiranje finansijskog unosa (samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        public IActionResult Create()
        {
            return View();
        }

        // 📌 POST: Dodavanje novog finansijskog unosa
        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Finansije finansije)
        {
            if (ModelState.IsValid)
            {
                await _finansijeRepo.AddAsync(finansije);
                return RedirectToAction(nameof(Index));
            }
            return View(finansije);
        }

        // 📌 GET: Prikaz forme za izmenu finansijskog unosa
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Edit(int id)
        {
            var finansije = await _finansijeRepo.GetByIdAsync(id);
            if (finansije == null)
            {
                return NotFound();
            }
            return View(finansije);
        }

        // 📌 POST: Ažuriranje finansijskog unosa
        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Finansije finansije)
        {
            if (id != finansije.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _finansijeRepo.UpdateAsync(finansije);
                return RedirectToAction(nameof(Index));
            }
            return View(finansije);
        }

        // 📌 GET: Prikaz detalja određenog finansijskog unosa
        public async Task<IActionResult> Details(int id)
        {
            var finansije = await _finansijeRepo.GetByIdAsync(id);
            if (finansije == null)
            {
                return NotFound();
            }
            return View(finansije);
        }

        // 📌 GET: Prikaz forme za brisanje
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Delete(int id)
        {
            var finansije = await _finansijeRepo.GetByIdAsync(id);
            if (finansije == null)
            {
                return NotFound();
            }
            return View(finansije);
        }

        // 📌 POST: Brisanje finansijskog unosa
        [Authorize(Roles = "Upravnik")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _finansijeRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
