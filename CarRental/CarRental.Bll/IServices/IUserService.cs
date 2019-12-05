using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetUsersAsync(UserFilter filter = null);

        Task<UserDto> GetUser(int? id);

        Task<UserDetailsDto> GetUserDetails(int? id);

        Task DeleteUser(int? userId);

        Task SetCultureInfoAsync(int? id, string cultureInfo);
    }
}
