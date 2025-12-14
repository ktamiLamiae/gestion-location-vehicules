using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace GestionLocationVehicule.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}