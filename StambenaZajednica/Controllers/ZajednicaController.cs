using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Controllers
{
    public class ZajednicaController : Controller
    {
        private readonly IStambenaZajednicaRepository _zajednicaRepo;

        public ZajednicaController(IStambenaZajednicaRepository zajednicaRepo)
        {
            _zajednicaRepo = zajednicaRepo;
        }

        public async Task<IActionResult> Index()
        {
            var zajednice = await _zajednicaRepo.GetAllAsync();
            return View(zajednice);
        }

        public IActionResult Create()
        {
            return View();
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var zajednica = await _zajednicaRepo.GetByIdAsync(id);
            if (zajednica == null) return NotFound();
            return View(zajednica);
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            var zajednica = await _zajednicaRepo.GetByIdAsync(id);
            if (zajednica == null) return NotFound();
            return View(zajednica);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _zajednicaRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
