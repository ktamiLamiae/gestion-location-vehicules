using Microsoft.AspNetCore.Mvc;

namespace GestionLocationVehicule.Areas.Client.Controllers
{
    [Area("Client")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
