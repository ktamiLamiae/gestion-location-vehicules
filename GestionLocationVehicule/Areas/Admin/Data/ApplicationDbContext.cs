using GestionLocationVehicule.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore; 

namespace GestionLocationVehicule.Areas.Admin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Vehicule> Vehicules { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Clients> Clients { get; set; }

    }
}
