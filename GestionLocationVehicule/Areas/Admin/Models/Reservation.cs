using GestionLocationVehicule.Areas.Admin.Enums;
using GestionLocationVehicule.Areas.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace GestionLocationVehicule.Areas.Admin.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int VehiculeId { get; set; }
        public Vehicule Vehicule { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Clients Client { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public decimal TotalPrice { get; set; }
        [Required]
        public ReservationStatut Statut;
    }
}
