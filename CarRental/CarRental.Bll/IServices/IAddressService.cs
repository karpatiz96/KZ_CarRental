using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAddresses();

        Task<AddressDto> GetAddress(int? id);

        Task<AddressDetailsDto> GetAddressDetails(int? id);

        Task<PagedResult<AddressDto>> GetAddresses(AddressFilter filter);

        Task CreateAddress(AddressInputDto addressDto);

        Task EditAddress(AddressInputDto addressDto);

        Task DeleteAddress(int? id);

        bool AddressExists(int? id);

        bool AddressHasReservations(int? id);
    }
}
