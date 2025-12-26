using GestionLocationVehicule.Areas.Client.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionLocationVehicule.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController(ApplicationClientDbContext context) : Controller
    {

        private readonly ApplicationClientDbContext _context = context;

        public IActionResult Index()
        {
            //return View();
            var vehicules = _context.Vehicules.Include(v=> v.VehicleCategory).ToList();
            return View(vehicules);
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

    }
}
