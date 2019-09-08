using CarRental.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.Services
{
    public interface IAddressService
    {

        Task<IEnumerable<AddressDto>> GetAddresses();

        Task<AddressDto> GetAddress(int? id);
    }
}
