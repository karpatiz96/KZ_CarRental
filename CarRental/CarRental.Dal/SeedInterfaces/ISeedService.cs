using CarRental.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.SeedInterfaces
{
    public interface ISeedService
    {
        IDictionary<string, VehicleModel> VehicleModels { get; }
        IList<Address> Addresses { get; }
        IList<Car> Cars { get; }
    }
}
