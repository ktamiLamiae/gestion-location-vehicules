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

    }
}
