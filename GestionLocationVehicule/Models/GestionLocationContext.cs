using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace GestionLocationVehicule.Models;

public partial class GestionLocationContext : DbContext
{
    public GestionLocationContext()
    {
    }

    public GestionLocationContext(DbContextOptions<GestionLocationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Vehiclecategory> Vehiclecategories { get; set; }

    public virtual DbSet<Vehicule> Vehicules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=gestion_location;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.Property(e => e.IdClient)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Adresse).HasMaxLength(50);
            entity.Property(e => e.Nom).HasMaxLength(50);
            entity.Property(e => e.NumeroTelephone).HasMaxLength(20);
            entity.Property(e => e.Prenom).HasMaxLength(50);
            entity.Property(e => e.Ville).HasMaxLength(50);
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Vehiclecategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vehiclecategories");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Libelle).HasColumnName("libelle");
        });

        modelBuilder.Entity<Vehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vehicules");

            entity.HasIndex(e => e.VehicleCategoryId, "IX_Vehicules_VehicleCategoryId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Kilometrage).HasColumnType("int(11)");
            entity.Property(e => e.Marque).HasMaxLength(50);
            entity.Property(e => e.Matricule).HasMaxLength(20);
            entity.Property(e => e.Modele).HasMaxLength(50);
            entity.Property(e => e.VehicleCategoryId).HasColumnType("int(11)");

            entity.HasOne(d => d.VehicleCategory).WithMany(p => p.Vehicules)
                .HasForeignKey(d => d.VehicleCategoryId)
                .HasConstraintName("FK_Vehicules_VehicleCategories_VehicleCategoryId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
