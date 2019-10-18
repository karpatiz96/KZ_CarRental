using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Bll.IServices;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public class UserService : IUserService
    {
        public CarRentalDbContext _dbContext { get; }

        private readonly UserManager<User> _userManager;

        public UserService(CarRentalDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Expression<Func<User, UserDto>> UserDtoSelector = u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        };

        public async Task<PagedResult<UserDto>> GetUsersAsync(UserFilter filter = null)
        {
            if (filter == null)
            {
                filter = new UserFilter();
            }

            if (filter?.PageSize < 0)
                filter.PageSize = null;
            if (filter?.PageNumber < 0)
                filter.PageNumber = null;

            IQueryable<User> users = _userManager.Users;

            switch (filter.userOrder)
            {
                case UserFilter.UserOrder.IdAscending:
                    users = users.OrderBy(u => u.Id);
                    break;
                case UserFilter.UserOrder.IdDescending:
                    users = users.OrderByDescending(u => u.Id);
                    break;
                case UserFilter.UserOrder.NameAscending:
                    users = users.OrderBy(u => u.Name);
                    break;
                case UserFilter.UserOrder.NameDescending:
                    users = users.OrderByDescending(u => u.Name);
                    break;
                case UserFilter.UserOrder.EmailAscending:
                    users = users.OrderBy(u => u.Email);
                    break;
                case UserFilter.UserOrder.EmailDescending:
                    users = users.OrderByDescending(u => u.Email);
                    break;
                default:
                    break;
            }

            int? Total = null;

            if (((filter?.PageSize) ?? 0) != 0)
            {
                filter.PageNumber = filter.PageNumber ?? 0;
                Total = users.Count();
                users = users.Skip(filter.PageNumber.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            var results = await users.Select(UserDtoSelector).ToListAsync();

            return new PagedResult<UserDto>
            {
                Total = Total,
                PageNumber = filter?.PageNumber,
                PageSize = filter?.PageSize,
                Results = results
            };
        }

        public async Task<UserDto> GetUser(int? id)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == id)
                .Select(UserDtoSelector)
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task<UserDetailsDto> GetUserDetails(int? id)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();

            if(user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userDetailsDto = new UserDetailsDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Roles = roles
                };

                return userDetailsDto;
            }

            return null;
        }
    }
}
