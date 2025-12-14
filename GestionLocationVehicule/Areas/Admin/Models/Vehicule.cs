using GestionLocationVehicule.Areas.Admin.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestionLocationVehicule.Areas.Admin.Models
{
    public class Vehicule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Matricule { get; set; }

        [MaxLength(50)]
        public string Marque { get; set; }

        [MaxLength(50)]
        public string Modele { get; set; }

        public int? Kilometrage { get; set; }

        //public Statut Statut { get; set; }
        public string Statut { get; set; }


        //public int Agence_Id { get; set; }
        public int VehicleCategoryId { get; set; }
        public VehicleCategory VehicleCategory { get; set; }
    }
}
