using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace StambenaZajednica.Controllers
{
    public class StanController : Controller
    {
        private readonly IStanRepository _stanRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public StanController(IStanRepository stanRepo, UserManager<ApplicationUser> userManager)
        {
            _stanRepo = stanRepo;
            _userManager = userManager;
        }

        // 📌 Prikaz svih stanova (dostupno samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Index()
        {
            var stanovi = await _stanRepo.GetAllAsync();
            var stanari = await _userManager.GetUsersInRoleAsync("Stanar");
            ViewBag.Stanari = await _userManager.GetUsersInRoleAsync("Stanar");
            return View(stanovi);
        }

        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public async Task<IActionResult> DodeliStanara(int stanId, string stanarId)
        {
            var stan = await _stanRepo.GetByIdAsync(stanId);
            if (stan == null) return NotFound();

            var stanar = await _userManager.FindByIdAsync(stanarId);
            if (stanar == null) return NotFound();

            stan.StanarId = stanar.Id;
            await _stanRepo.UpdateAsync(stan);

            return RedirectToAction(nameof(Index));
        }

        // 📌 Prikaz forme za kreiranje stana (samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        public IActionResult Create()
        {
            return View();
        }

        // 📌 POST: Dodavanje novog stana
        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public async Task<IActionResult> Create(Stan stan)
        {
            if (ModelState.IsValid)
            {
                await _stanRepo.AddAsync(stan);
                return RedirectToAction(nameof(Index));
            }
            return View(stan);
        }

        // 📌 Prikaz forme za izmenu stana (samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Edit(int id)
        {
            var stan = await _stanRepo.GetByIdAsync(id);
            if (stan == null) return NotFound();
            return View(stan);
        }

        // 📌 POST: Ažuriranje stana (samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Stan stan)
        {
            if (id != stan.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _stanRepo.UpdateAsync(stan);
                return RedirectToAction(nameof(Index));
            }
            return View(stan);
        }

        // 📌 Prikaz forme za brisanje stana (samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Delete(int id)
        {
            var stan = await _stanRepo.GetByIdAsync(id);
            if (stan == null) return NotFound();
            return View(stan);
        }

        // 📌 POST: Brisanje stana (samo za upravnika)
        [Authorize(Roles = "Upravnik")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stanRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
