using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal
{
    public class CarRentalDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        private readonly Lazy<IEntityTypeConfiguration<Address>> _addressConfiguration;
        private readonly Lazy<IEntityTypeConfiguration<Car>> _carConfiguration;
        private readonly Lazy<IEntityTypeConfiguration<VehicleModel>> _vehicleModelConfiguration;

        public CarRentalDbContext(DbContextOptions options, Lazy<IEntityTypeConfiguration<Address>> addressConfiguration,
            Lazy<IEntityTypeConfiguration<Car>> carConfiguration, Lazy<IEntityTypeConfiguration<VehicleModel>> vehicleModelConfiguration) : base(options)
        {
            _addressConfiguration = addressConfiguration;
            _carConfiguration = carConfiguration;
            _vehicleModelConfiguration = vehicleModelConfiguration;
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(_addressConfiguration.Value);
            modelBuilder.ApplyConfiguration(_carConfiguration.Value);
            modelBuilder.ApplyConfiguration(_vehicleModelConfiguration.Value);

            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
