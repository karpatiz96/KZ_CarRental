using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class CarService : ICarService
    {
        public CarRentalDbContext _dbContext { get; }

        public CarService(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<Car, CarDto>> CarDtoSelector { get; } = c => new CarDto
        {
            Id = c.Id,
            PlateNumber = c.PlateNumber,
            VehicleModelId = c.VehicleModelId,
            VehicleType = c.VehicleModel.VehicleType,
            AddressId = c.AddressId,
            Address = c.AddressId.HasValue ?
                c.Address.ZipCode.ToString() + " " + c.Address.City + " " + c.Address.StreetAddress
                : "",
            Active = c.Active
        };

        public static Expression<Func<Car, CarDetailsDto>> CarDetailsDtoSelector { get; } = c => new CarDetailsDto
        {
            Id = c.Id,
            PlateNumber = c.PlateNumber,
            VehicleModelId = c.VehicleModelId,
            VehicleType = c.VehicleModel.VehicleType,
            AddressId = c.AddressId,
            Address = c.AddressId.HasValue ?
                c.Address.ZipCode.ToString() + " " + c.Address.City + " " + c.Address.StreetAddress
                : "",
            Active = c.Active,
            HasReservation = c.Reservations.Any()
        };

        public async Task<PagedResult<CarDto>> GetCars(CarFilter filter)
        {
            if (filter == null)
            {
                filter = new CarFilter();
            }

            if (filter?.PageSize < 0)
                filter.PageSize = null;
            if (filter?.PageNumber < 0)
                filter.PageNumber = null;

            IQueryable<Car> cars = _dbContext.Cars
                .Include(c => c.VehicleModel)
                .Include(c => c.Address);

            if (!string.IsNullOrWhiteSpace(filter?.VehicleType))
                cars = cars.Where(c => c.VehicleModel.VehicleType.Contains(filter.VehicleType));

            if (!string.IsNullOrEmpty(filter.PlateNumber))
                cars = cars.Where(c => c.PlateNumber.Contains(filter.PlateNumber));

            switch (filter.carOrder)
            {
                case CarFilter.CarOrder.PlateNumberAscending:
                    cars = cars.OrderBy(c => c.PlateNumber);
                    break;
                case CarFilter.CarOrder.PlateNumberDescending:
                    cars = cars.OrderByDescending(c => c.PlateNumber);
                    break;
                case CarFilter.CarOrder.VehicleTypeAscending:
                    cars = cars.OrderBy(c => c.VehicleModel.VehicleType);
                    break;
                case CarFilter.CarOrder.VehicleTypeDescending:
                    cars = cars.OrderByDescending(c => c.VehicleModel.VehicleType);
                    break;
                case CarFilter.CarOrder.ActiveAscending:
                    cars = cars.OrderBy(c => c.Active);
                    break;
                case CarFilter.CarOrder.ActiveDescending:
                    cars = cars.OrderByDescending(c => c.Active);
                    break;
                case CarFilter.CarOrder.AddressAscending:
                    cars = cars.OrderBy(c => c.Address.ZipCode).OrderBy(c => c.Address.City).OrderBy(c => c.Address.StreetAddress);
                    break;
                case CarFilter.CarOrder.AddressDescending:
                    cars = cars.OrderByDescending(c => c.Address.ZipCode).OrderByDescending(c => c.Address.City).OrderByDescending(c => c.Address.StreetAddress);
                    break;
                default:
                    cars = cars.OrderBy(c => c.PlateNumber);
                    break;
            }

            int? Total = null;

            if (((filter?.PageSize) ?? 0) != 0)
            {
                filter.PageNumber = filter.PageNumber ?? 0;
                Total = cars.Count();
                cars = cars.Skip(filter.PageNumber.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            var results = await cars
                .Select(CarDtoSelector)
                .ToListAsync();

            return new PagedResult<CarDto>
            {
                Total = Total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Results = results
            };
        }

        public async Task<CarDto> GetCar(int? id)
        {
            return await _dbContext.Cars
                .Where(c => c.Id == id.Value)
                .Select(CarDtoSelector)
                .SingleOrDefaultAsync();
        }

        public async Task CreateCar(CarDto carDto)
        {
            /*VehicleModel vehicleModel = await _dbContext.VehicleModels
                .Where(vm => vm.Id == carDto.VehicleModelId)
                .SingleOrDefaultAsync();*/

            Car car = new Car
            {
                PlateNumber = carDto.PlateNumber,
                Active = carDto.Active,
                VehicleModelId = carDto.VehicleModelId,
                //VehicleModel = vehicleModel,
                AddressId = carDto.AddressId
            };

            _dbContext.Add(car);
            //vehicleModel.Cars.Add(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditCar(CarDto carDto)
        {
            var car = await _dbContext.Cars
                .Include(c => c.VehicleModel)
                    .ThenInclude(vm => vm.Cars)
                .Include(c => c.Reservations)
                .Include(c => c.Address)
                .Where(c => c.Id == carDto.Id)
                .SingleOrDefaultAsync();

            var vehicleModel = await _dbContext.VehicleModels
                .Include(vm => vm.Cars)
                .Where(vm => vm.Id == carDto.VehicleModelId)
                .SingleOrDefaultAsync();

            var address = await _dbContext.Addresses
                .Include(a => a.Cars)
                .Where(a => a.Id == carDto.AddressId)
                .SingleOrDefaultAsync();


            car.Active = carDto.Active;
            car.PlateNumber = carDto.PlateNumber;

            if (car.VehicleModelId != carDto.VehicleModelId)
            {
                car.VehicleModel.Cars.Remove(car);
                car.VehicleModel = vehicleModel;
                vehicleModel.Cars.Add(car);
            }

            if(car.AddressId != carDto.AddressId)
            {
                car.Address.Cars.Remove(car);
                car.Address = address;
                car.Address.Cars.Add(car);
            }

            _dbContext.Attach(car).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCar(int? id)
        {
            var car = await _dbContext.Cars
                .Include(c => c.VehicleModel)
                .Include(c => c.Reservations)
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();

            var vehicleModel = await _dbContext.VehicleModels
                .Include(vm => vm.Cars)
                .Where(vm => vm.Id == car.VehicleModelId)
                .SingleOrDefaultAsync();

            var address = await _dbContext.Addresses
                .Include(a => a.Cars)
                .Where(a => a.Id == car.AddressId)
                .SingleOrDefaultAsync();

            if(address != null)
            {
                address.Cars.Remove(car);
            }

            vehicleModel.Cars.Remove(car);
            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Car>> GetCars(DateTime start, DateTime end, int? id)
        {
            IQueryable<Car> cars = _dbContext.Cars
                .Include(c => c.Reservations)
                .Include(c => c.VehicleModel)
                .Where(c => c.Active == true);

            var carList = await cars
                .Where(c => c.VehicleModel.Active == true && c.VehicleModelId == id)
                .Where(c => c.Reservations
                .Where(r => (r.PickUpTime.Date <= start.Date && r.DropOffTime.Date >= start.Date)
                || (end.Date <= r.DropOffTime.Date && end.Date >= r.PickUpTime.Date))
                .Any(r => r.State == Reservation.ReservationStates.Accepted) == false)
                .ToListAsync();

            return carList;
        }

        public async Task<bool> CarHasReservations(int? id)
        {
            var car = await _dbContext.Cars
                .Include(c => c.Reservations)
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();

            if (car.Reservations.Any())
            {
                return true;
            }

            return false;
        }

        public bool CarExists(int? id)
        {
            return _dbContext.Cars.Any(e => e.Id == id);
        }

        public async Task<CarDetailsDto> GetCarDetailsDto(int? id)
        {
            return await _dbContext.Cars
                .Include(c => c.Reservations)
                .Include(c => c.Address)
                .Where(c => c.Id == id.Value)
                .Select(CarDetailsDtoSelector)
                .SingleOrDefaultAsync();
        }
    }
}
