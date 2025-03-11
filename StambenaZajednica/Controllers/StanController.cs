using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Controllers
{
    public class StanController : Controller
    {
        private readonly IStanRepository _stanRepo;

        public StanController(IStanRepository stanRepo)
        {
            _stanRepo = stanRepo;
        }

        public async Task<IActionResult> Index()
        {
            var stanovi = await _stanRepo.GetAllAsync();
            return View(stanovi);
        }

        public IActionResult Create()
        {
            return View();
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var stan = await _stanRepo.GetByIdAsync(id);
            if (stan == null) return NotFound();
            return View(stan);
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            var stan = await _stanRepo.GetByIdAsync(id);
            if (stan == null) return NotFound();
            return View(stan);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stanRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
