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
    public class AddressService : IAddressService
    {
        public CarRentalDbContext _dbContext { get; }

        public AddressService(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static Expression<Func<Address, AddressDto>> AddressDtoSelector { get; } = a => new AddressDto
        {
            Id = a.Id,
            ZipCode = a.ZipCode,
            City = a.City,
            StreetAddress = a.StreetAddress,
            FullAddress = a.ZipCode.ToString() + " " + a.City + " " + a.StreetAddress
        };

        public static Expression<Func<Address, AddressDetailsDto>> AddressDetailsDtoSelector { get; } = a => new AddressDetailsDto
        {
            Id = a.Id,
            ZipCode = a.ZipCode,
            City = a.City,
            StreetAddress = a.StreetAddress,
            FullAddress = a.ZipCode.ToString() + " " + a.City + " " + a.StreetAddress,
            IsInUse = a.IsInUse,
            Cars = a.Cars.Select(c => new CarDto
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                VehicleModelId = c.VehicleModelId,
                VehicleType = c.VehicleModel.VehicleType,
                AddressId = a.Id,
                Address = a.ZipCode.ToString() + " " + a.City + " " + a.StreetAddress,
                Active = c.Active
            }).ToList(),
            CarFound = a.Cars.Count,
            HasReservation = a.Reservations.Any()
        };

        public async Task<IEnumerable<AddressDto>> GetAddresses()
        {
            return await _dbContext.Addresses
                .Where(a => a.IsInUse == true)
                .Select(AddressDtoSelector)
                .ToListAsync();
        }

        public async Task<AddressDto> GetAddress(int? id)
        {
            return await _dbContext.Addresses
                .Where(vm => vm.Id == id)
                .Select(AddressDtoSelector)
                .SingleOrDefaultAsync();
        }

        public async Task<AddressDetailsDto> GetAddressDetails(int? id)
        {
            return await _dbContext.Addresses
                .Include(a => a.Cars)
                .Include(a => a.Reservations)
                .Where(vm => vm.Id == id)
                .Select(AddressDetailsDtoSelector)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<PagedResult<AddressDto>> GetAddresses(AddressFilter filter)
        {
            if (filter == null)
            {
                filter = new AddressFilter();
            }

            if (filter?.PageSize < 0)
                filter.PageSize = null;
            if (filter?.PageNumber < 0)
                filter.PageNumber = null;

            IQueryable<Address> addresses = _dbContext.Addresses;

            switch (filter.addressOrder)
            {
                case AddressFilter.AddressOrder.ZipCodeAscending:
                    addresses = addresses.OrderBy(a => a.ZipCode);
                    break;
                case AddressFilter.AddressOrder.ZipCodeDescending:
                    addresses = addresses.OrderByDescending(a => a.ZipCode);
                    break;
                case AddressFilter.AddressOrder.CityAscending:
                    addresses = addresses.OrderBy(a => a.City);
                    break;
                case AddressFilter.AddressOrder.CityDescending:
                    addresses = addresses.OrderByDescending(a => a.City);
                    break;
                case AddressFilter.AddressOrder.StreetAddressAscending:
                    addresses = addresses.OrderBy(a => a.StreetAddress);
                    break;
                case AddressFilter.AddressOrder.StreetAddressDescending:
                    addresses = addresses.OrderByDescending(a => a.StreetAddress);
                    break;
                default:
                    addresses = addresses.OrderBy(a => a.ZipCode);
                    break;
            }

            int? Total = null;

            if (((filter?.PageSize) ?? 0) != 0)
            {
                filter.PageNumber = filter.PageNumber ?? 0;
                Total = addresses.Count();
                addresses = addresses.Skip(filter.PageNumber.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            var results = await addresses
                .Select(AddressDtoSelector)
                .ToListAsync();

            return new PagedResult<AddressDto>
            {
                Total = Total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Results = results
            };
        }

        public async Task CreateAddress(AddressInputDto addressDto)
        {
            Address address = new Address
            {
                City = addressDto.City,
                ZipCode = addressDto.ZipCode.Value,
                StreetAddress = addressDto.StreetAddress,
                IsInUse = addressDto.IsInUse
            };

            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAddress(AddressInputDto addressDto)
        {
            var address = await _dbContext.Addresses
                .Where(a => a.Id == addressDto.Id)
                .SingleOrDefaultAsync();

            address.City = addressDto.City;
            address.ZipCode = addressDto.ZipCode.Value;
            address.StreetAddress = address.StreetAddress;

            _dbContext.Attach(address).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAddress(int? id)
        {
            var address = await _dbContext.Addresses
                .Include(a => a.Cars)
                .Where(a => a.Id == id.Value)
                .SingleOrDefaultAsync();

            var cars = await _dbContext.Cars
                .Include(c => c.Address)
                .Where(c => c.AddressId == id)
                .ToListAsync();

            foreach (var car in cars)
            {
                address.Cars.Remove(car);
                car.Address = null;
            }

            _dbContext.Addresses.Remove(address);

            await _dbContext.SaveChangesAsync();
        }

        public bool AddressHasReservations(int? id)
        {
            var result = _dbContext.Reservations
                .Where(r => r.AddressId == id)
                .Any();

            return result;
        }

        public bool AddressExists(int? id)
        {
            return _dbContext.Addresses.Where(a => a.IsInUse == true).Any(e => e.Id == id);
        }
    }
}
