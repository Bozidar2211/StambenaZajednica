using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;
using Microsoft.AspNetCore.Authorization;

namespace StambenaZajednica.Controllers
{
    public class ZajednicaController : Controller
    {
        private readonly IStambenaZajednicaRepository _zajednicaRepo;

        public ZajednicaController(IStambenaZajednicaRepository zajednicaRepo)
        {
            _zajednicaRepo = zajednicaRepo;
        }

        // 📌 Prikaz svih stambenih zajednica - dostupno svima (Stanar i Upravnik)
        [Authorize(Roles = "Stanar,Upravnik")]
        public async Task<IActionResult> Index()
        {
            var zajednice = await _zajednicaRepo.GetAllAsync();
            return View(zajednice);
        }

        // 📌 Prikaz forme za kreiranje stambene zajednice - samo za upravnika
        [Authorize(Roles = "Upravnik")]
        public IActionResult Create()
        {
            return View();
        }

        // 📌 POST: Kreiranje stambene zajednice - samo za upravnika
        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public async Task<IActionResult> Create(StambZajednica zajednica)
        {
            if (ModelState.IsValid)
            {
                await _zajednicaRepo.AddAsync(zajednica);
                return RedirectToAction(nameof(Index));
            }
            return View(zajednica);
        }

        // 📌 Prikaz forme za izmenu stambene zajednice - samo za upravnika
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Edit(int id)
        {
            var zajednica = await _zajednicaRepo.GetByIdAsync(id);
            if (zajednica == null) return NotFound();
            return View(zajednica);
        }

        // 📌 POST: Ažuriranje stambene zajednice - samo za upravnika
        [Authorize(Roles = "Upravnik")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, StambZajednica zajednica)
        {
            if (id != zajednica.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _zajednicaRepo.UpdateAsync(zajednica);
                return RedirectToAction(nameof(Index));
            }
            return View(zajednica);
        }

        // 📌 Prikaz forme za brisanje stambene zajednice - samo za upravnika
        [Authorize(Roles = "Upravnik")]
        public async Task<IActionResult> Delete(int id)
        {
            var zajednica = await _zajednicaRepo.GetByIdAsync(id);
            if (zajednica == null) return NotFound();
            return View(zajednica);
        }

        // 📌 POST: Brisanje stambene zajednice - samo za upravnika
        [Authorize(Roles = "Upravnik")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _zajednicaRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
