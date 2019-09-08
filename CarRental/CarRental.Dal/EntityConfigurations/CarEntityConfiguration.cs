using CarRental.Dal.Entities;
using CarRental.Dal.SeedInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.EntityConfigurations
{
    public class CarEntityConfiguration : IEntityTypeConfiguration<Car>
    {
        private readonly Lazy<ISeedService> _seedService;

        public CarEntityConfiguration(Lazy<ISeedService> seedService)
        {
            _seedService = seedService;
        }

        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasData(_seedService.Value.Cars);
        }
    }
}
