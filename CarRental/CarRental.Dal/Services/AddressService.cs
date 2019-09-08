using CarRental.Dal.Dtos;
using CarRental.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.Services
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

        public async Task<IEnumerable<AddressDto>> GetAddresses()
        {
            return await _dbContext.Addresses.Select(AddressDtoSelector).ToAsyncEnumerable().ToList();
        }

        public async Task<AddressDto> GetAddress(int? id)
        {
            return await _dbContext.Addresses.Where(vm => vm.Id == id).Select(AddressDtoSelector).FirstOrDefaultAsync();
        }
    }
}
