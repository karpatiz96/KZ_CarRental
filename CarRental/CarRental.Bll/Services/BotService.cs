using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class BotService : IBotService 
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BotService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public static Expression<Func<Address, AddressDto>> AddressDtoSelector { get; } = a => new AddressDto
        {
            Id = a.Id,
            Name = a.Name,
            ZipCode = a.ZipCode,
            City = a.City,
            StreetAddress = a.StreetAddress,
            FullAddress = a.ZipCode.ToString() + " " + a.City + " " + a.StreetAddress
        };

        public static Expression<Func<VehicleModel, VehicleModelNameDto>> VehicleModelNameDtoSelector { get; } = v => new VehicleModelNameDto
        {
            VehicleType = v.VehicleType,
            Cars = v.Cars.Count
        };

        public async Task<IEnumerable<Car>> GetCars(DateTime start, DateTime end, string model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();

                IQueryable<Car> cars = _dbContext.Cars
                .Include(c => c.Reservations)
                .Include(c => c.VehicleModel)
                .Where(c => c.Active == true);

                var vehicleModel = await _dbContext.VehicleModels.Where(vm => vm.VehicleType.Contains(model)).FirstOrDefaultAsync();
                var id = vehicleModel?.Id;

                var carList = await cars
                    .Where(c => c.VehicleModel.Active == true && c.VehicleModel.Id == id)
                    .Where(c => c.Reservations
                    .Where(r => (r.PickUpTime.Date <= start.Date && r.DropOffTime.Date >= start.Date)
                    || (end.Date <= r.DropOffTime.Date && end.Date >= r.PickUpTime.Date))
                    .Any(r => r.State == Reservation.ReservationStates.Accepted) == false)
                    .ToListAsync();

                return carList;
            }
        }

        public async Task<IEnumerable<AddressDto>> GetAddresses()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();

                return await _dbContext.Addresses
                .Where(a => a.IsInUse == true)
                .Select(AddressDtoSelector)
                .ToListAsync();
            }
        }

        public async Task<IEnumerable<VehicleModelNameDto>> GetFreeVehicles()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();

                IQueryable<VehicleModel> vehicleModels = _dbContext.VehicleModels
                .Include(vm => vm.Cars)
                    .ThenInclude(c => c.Reservations)
                .Where(vm => vm.Active == true);

                var start = DateTime.Now;
                var end = DateTime.Now.AddDays(7);

                var vehicleList = await vehicleModels
                    .Where(vm => vm.Cars
                        .Where(c => c.Reservations
                            .Where(r => (r.PickUpTime.Date <= start.Date && r.DropOffTime.Date >= start.Date)
                                || (end.Date <= r.DropOffTime.Date && end.Date >= r.PickUpTime.Date))
                        .Any(r => r.State == Reservation.ReservationStates.Accepted) == false).Any()
                    )
                    .Select(VehicleModelNameDtoSelector)
                    .ToListAsync();

                return vehicleList;
            }
        }
    }
}
