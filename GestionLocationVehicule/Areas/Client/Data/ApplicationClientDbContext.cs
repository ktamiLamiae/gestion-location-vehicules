using GestionLocationVehicule.Areas.Client.Models;
using Microsoft.EntityFrameworkCore; 

namespace GestionLocationVehicule.Areas.Client.Data
{
    public class ApplicationClientDbContext : DbContext
    {
        public ApplicationClientDbContext(DbContextOptions<ApplicationClientDbContext> options) : base(options)
        { }

        public DbSet<Vehicule> Vehicules { get; set; }
        public DbSet<VehicleCategory> VehicleCategories { get; set; }

    }
}
