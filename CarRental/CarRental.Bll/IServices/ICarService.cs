using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface ICarService
    {
        Task<PagedResult<CarDto>> GetCars(CarFilter filter);

        Task<CarDto> GetCar(int? id);

        Task CreateCar(CarDto carDto);

        Task EditCar(CarDto carDto);

        Task DeleteCar(int? id);

        Task<IEnumerable<Car>> GetCars(DateTime start, DateTime end, int? id);

        Task<bool> CarHasReservations(int? id);

        bool CarExists(int? id);

        Task<CarDetailsDto>  GetCarDetailsDto(int? id);
    }
}
