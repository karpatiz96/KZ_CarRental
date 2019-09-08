using CarRental.Dal.Entities;
using CarRental.Dal.SeedInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.EntityConfigurations
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        private readonly Lazy<ISeedService> _seedService;

        public AddressEntityConfiguration(Lazy<ISeedService> seedService)
        {
            _seedService = seedService;
        }

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasData(_seedService.Value.Addresses);
        }
    }
}
