using CarRental.Bll.Dtos;
using CarRental.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface IBotService
    {
        Task<IEnumerable<Car>> GetCars(DateTime start, DateTime end, string model);

        Task<IEnumerable<AddressDto>> GetAddresses();

        Task<IEnumerable<VehicleModelNameDto>> GetFreeVehicles();
    }
}
