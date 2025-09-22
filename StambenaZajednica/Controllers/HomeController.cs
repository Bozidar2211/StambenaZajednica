using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StambenaZajednica.Data.RepositoryInterfaces;
using StambenaZajednica.Models;

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStanRepository _stanRepo;

    public HomeController(UserManager<ApplicationUser> userManager, IStanRepository stanRepo)
    {
        _userManager = userManager;
        _stanRepo = stanRepo;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user?.StanId != null)
        {
            var stan = _stanRepo.GetStanById(user.StanId.Value);
            if (stan != null
                && stan.ObavestenONovomPINu == false
                && stan.DatumOtkljucavanjaFinansija != null) 
            {
                ViewBag.Poruka = $"Vaš PIN je resetovan. Novi PIN je: {stan.Pin}";
                stan.ObavestenONovomPINu = true;
                _stanRepo.UpdateStan(stan);
            }
        }

        return View();
    }

}
