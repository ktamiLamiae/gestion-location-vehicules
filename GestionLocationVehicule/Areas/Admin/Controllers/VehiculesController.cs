using GestionLocationVehicule.Areas.Admin.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;

namespace GestionLocationVehicule.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VehiculesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiculesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vehicules = _context.Vehicules
                                .Include(v => v.VehicleCategory)
                                .ToList();
            //ViewBag.vehicules = vehicules;
            return View(vehicules);
        }
    }
}
