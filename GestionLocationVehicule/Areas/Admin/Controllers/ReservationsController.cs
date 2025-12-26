using GestionLocationVehicule.Areas.Admin.Data;
using GestionLocationVehicule.Areas.Admin.Enums;
using GestionLocationVehicule.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionLocationVehicule.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext context;
        //private readonly GestionLocationContext _conteext;

        public ReservationsController(ApplicationDbContext context)
        {
            this.context = context;
            //this._conteext = _conteext;
        }
        public IActionResult Create()
        {

            var vehicules = context.Vehicules.Where(v => v.Statut == Statut.Disponible).ToList();
            ViewBag.Vehicules = vehicules;
            var clients = context.Clients.ToList();
            ViewBag.Clients = clients;
            return View();
        }

        public decimal CalculerPrixTotal(DateTime startDate, DateTime endDate, Decimal prix)
        {
            if (prix <= 0)
            {
                return 0;
            }
            int numberOfDays = (endDate - startDate).Days + 1;
            decimal totalPrice = prix * numberOfDays;

            return totalPrice;
        }
        [HttpPost]
        public IActionResult ConfirmReseration(int ClientId, int VehiculeId, DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ViewBag.ErrMessage = "Informations invalides";
                return View("Create", "Reservations");
                //return Json("Informations invalides");
            }
            if (endDate < startDate)
            {
                ViewBag.ErrMessage = "La date de fin doit être après la date de début";
                return View("Create", "Reservations");
                //return Json("La date de fin doit être après la date de début");
            }
            var client = context.Clients.FirstOrDefault(c => c.IdClient == ClientId);
            var vehicule = context.Vehicules.FirstOrDefault(v => v.Id == VehiculeId);

            if (client == null || vehicule == null)
            {
                ViewBag.ErrMessage = "Client ou véhicule introuvable";
                return View("Create", "Reservations");
                //return Json("Client ou véhicule introuvable");
            }
            Reservation r = new Reservation();
            r.ClientId = ClientId;
            r.VehiculeId = VehiculeId;
            r.StartDate = (DateTime)startDate;
            r.EndDate = (DateTime)endDate;
            r.TotalPrice = this.CalculerPrixTotal(startDate.Value, endDate.Value, vehicule.Prix);
            r.Statut = ReservationStatut.Reserved;

            //var vehicule = context.Vehicules.FirstOrDefault(v => v.Id == VehiculeId);
            if (vehicule != null && r.Statut == ReservationStatut.Reserved)
            {
                vehicule.Statut = Enums.Statut.EnLocation;
                //context.SaveChanges();
            }

            ViewBag.Success = "Réservation bien effectuée et sauvegardée";
            context.Reservations.Add(r);
            context.SaveChanges();

            return View("Create", "Reservations");
            //return Json("Good");
        }
        [HttpGet]
        public IActionResult ListReservations()
        {

            var reservations = context.Reservations
                                       .Include(r => r.Vehicule)
                                       .Include(r => r.Client)
                                       .ToList();

            return View(reservations);
        }
    }
}
