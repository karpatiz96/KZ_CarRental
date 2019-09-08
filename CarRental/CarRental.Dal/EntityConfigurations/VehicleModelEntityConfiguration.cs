using CarRental.Dal.Entities;
using CarRental.Dal.SeedInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.EntityConfigurations
{
    public class VehicleModelEntityConfiguration : IEntityTypeConfiguration<VehicleModel>
    {
        private readonly Lazy<ISeedService> _seedService;

        public VehicleModelEntityConfiguration(Lazy<ISeedService> seedService)
        {
            _seedService = seedService;
        }

        public void Configure(EntityTypeBuilder<VehicleModel> builder)
        {
            builder.HasData(_seedService.Value.VehicleModels.Values);
        }
    }
}
