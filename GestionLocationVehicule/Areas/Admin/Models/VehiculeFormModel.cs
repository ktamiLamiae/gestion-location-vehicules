using GestionLocationVehicule.Areas.Admin.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GestionLocationVehicule.Areas.Admin.Models
{
    public class VehiculeFormModel
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Titre { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required, MaxLength(20)]
        public string Matricule { get; set; }
        [Required, MaxLength(20)]
        public string Marque { get; set; }
        [Required, MaxLength(20)]
        public string Modele { get; set; }
        public Statut Statut { get; set; }
        [Required]
        [Range(1980, 2100)]
        public int Annee { get; set; }
        public int? Kilometrage { get; set; }
        [Required]
        public decimal Prix { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int VehicleCategoryId { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
    }



}
