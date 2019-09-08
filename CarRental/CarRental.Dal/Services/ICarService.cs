using CarRental.Dal.Dtos;
using CarRental.Dal.Entities;
using CarRental.Dal.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.Services
{
    public interface ICarService
    {

        Task<PagedResult<CarDto>> GetCars(CarFilter filter);

        Task<CarDto> GetCar(int? id);

        Task CreateCar(CarDto carDto);

        Task EditCar(CarDto carDto);

        Task DeleteCar(int? id);

        Task<IEnumerable<Car>> GetCars(DateTime start, DateTime end, int? id);

        Task<IEnumerable<CarDto>> GetCarList(int? vehicleModelId);

        Task<bool> CarHasReservations(int? id);

        bool CarExists(int? id);
    }
}
