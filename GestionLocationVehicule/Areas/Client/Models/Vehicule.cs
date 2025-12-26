using GestionLocationVehicule.Areas.Client.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GestionLocationVehicule.Areas.Client.Models
{
    public class Vehicule
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string? Description { get; set; }
        public string Matricule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public int? Kilometrage { get; set; }
        public Statut Statut { get; set; }
        public string ImagePath { get; set; } = "";
        public decimal Prix { get; set; }
        public int Annee { get; set; }
        public VehicleCategory VehicleCategory { get; set; }

    }
}
