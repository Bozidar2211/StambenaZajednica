using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

namespace StambenaZajednica.Controllers
{
    [Authorize(Roles = "Upravnik")]
    public class ZajednicaController : Controller
    {
        private readonly IStambenaZajednicaRepository _repo;

        public ZajednicaController(IStambenaZajednicaRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var zajednice = _repo.GetSveZajednice();
            return View(zajednice);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StambZajednica zajednica)
        {
            if (!ModelState.IsValid) return View(zajednica);

            _repo.AddZajednica(zajednica);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var zajednica = _repo.GetZajednicaById(id);
            if (zajednica == null) return NotFound();

            return View(zajednica);
        }

        [HttpPost]
        public IActionResult Edit(StambZajednica zajednica)
        {
            if (!ModelState.IsValid) return View(zajednica);

            _repo.UpdateZajednica(zajednica);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var zajednica = _repo.GetZajednicaById(id);
            if (zajednica == null) return NotFound();

            return View(zajednica);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteZajednica(id);
            return RedirectToAction("Index");
        }
    }
}
