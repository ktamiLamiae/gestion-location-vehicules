using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLocationVehicule.Models
{
    public partial class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClient { get; set; }

        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Adresse { get; set; } = null!;
        public string Ville { get; set; } = null!;
        public string NumeroTelephone { get; set; } = null!;

        // Nouvelles propriétés ajoutées
        public string Email { get; set; } = null!;
        public string MotDePasse { get; set; } = null!;
        public string TypeUtilisateur { get; set; } = "Client"; // Valeur par défaut
    }
}
