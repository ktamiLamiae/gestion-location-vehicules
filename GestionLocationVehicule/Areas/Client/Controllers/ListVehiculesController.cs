using GestionLocationVehicule.Areas.Client.Data;
using GestionLocationVehicule.Areas.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestionLocationVehicule.Areas.Client.Controllers
{
    [Area("Client")]
    public class ListVehiculesController(ApplicationClientDbContext context) : Controller
    {

        private readonly ApplicationClientDbContext _context = context;

        public IActionResult Index(string? searchInput, int? statusFilter)
        {
            var vehicules = _context.Vehicules.Include(v => v.VehicleCategory).AsQueryable();
            if (!string.IsNullOrEmpty(searchInput))
            {
                vehicules = vehicules.Where(v =>
                    v.Titre.Contains(searchInput) ||
                    v.Modele.Contains(searchInput) ||
                    v.Marque.Contains(searchInput));
            }
            if (statusFilter.HasValue)
            {
                vehicules = vehicules.Where(v => (int)v.Statut == statusFilter.Value);
            }
            //var vehicules = _context.Vehicules.Include(v => v.VehicleCategory).ToList();
            return View(vehicules.ToList());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var vehicule = _context.Vehicules
                .Include(v => v.VehicleCategory)
                .FirstOrDefault(v => v.Id == id);

            if (vehicule == null)
            {
                TempData["Error"] = "Véhicule introuvable.";
                return RedirectToAction("Index", "ListVehicules");
            }
            return View(vehicule);
        }

    }
}
