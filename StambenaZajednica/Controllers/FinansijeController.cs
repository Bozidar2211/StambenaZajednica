using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Controllers
{
    public class FinansijeController : Controller
    {
        private readonly IFinansijeRepository _finansijeRepo;

        public FinansijeController(IFinansijeRepository finansijeRepo)
        {
            _finansijeRepo = finansijeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var racuni = await _finansijeRepo.GetAllAsync();
            return View(racuni);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Finansije racun)
        {
            if (ModelState.IsValid)
            {
                await _finansijeRepo.AddAsync(racun);
                return RedirectToAction(nameof(Index));
            }
            return View(racun);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var racun = await _finansijeRepo.GetByIdAsync(id);
            if (racun == null) return NotFound();
            return View(racun);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Finansije racun)
        {
            if (id != racun.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _finansijeRepo.UpdateAsync(racun);
                return RedirectToAction(nameof(Index));
            }
            return View(racun);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var racun = await _finansijeRepo.GetByIdAsync(id);
            if (racun == null) return NotFound();
            return View(racun);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _finansijeRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
