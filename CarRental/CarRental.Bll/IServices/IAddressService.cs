using CarRental.Bll.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAddresses();

        Task<AddressDto> GetAddress(int? id);
    }
}
