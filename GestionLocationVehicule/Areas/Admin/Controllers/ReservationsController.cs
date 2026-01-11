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


        [HttpGet]
        public IActionResult Details(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Vehicule)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var reservation = context.Reservations.Find(id);
            if (reservation == null)
                return NotFound();

            ViewBag.Vehicules = context.Vehicules.ToList();
            ViewBag.Clients = context.Clients.ToList();

            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(int id, int ClientId, int VehiculeId, DateTime startDate, DateTime endDate, ReservationStatut statut)
        {
            var reservation = context.Reservations
                .Include(r => r.Vehicule)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            reservation.ClientId = ClientId;
            reservation.VehiculeId = VehiculeId;
            reservation.StartDate = startDate;
            reservation.EndDate = endDate;
            reservation.Statut = statut;
            reservation.TotalPrice = CalculerPrixTotal(startDate, endDate, reservation.Vehicule.Prix);

            context.SaveChanges();

            return RedirectToAction(nameof(ListReservations));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Vehicule)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.Vehicule)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            reservation.Vehicule.Statut = Statut.Disponible;
            context.Reservations.Remove(reservation);
            context.SaveChanges();

            return RedirectToAction(nameof(ListReservations));
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.Vehicule)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            reservation.Statut = ReservationStatut.Cancelled;
            reservation.Vehicule.Statut = Statut.Disponible;

            context.SaveChanges();

            return RedirectToAction(nameof(ListReservations));
        }

        [HttpPost]
        public IActionResult Complete(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.Vehicule)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            reservation.Statut = ReservationStatut.Completed;
            reservation.Vehicule.Statut = Statut.Disponible;

            context.SaveChanges();

            return RedirectToAction(nameof(ListReservations));
        }

    }
}
