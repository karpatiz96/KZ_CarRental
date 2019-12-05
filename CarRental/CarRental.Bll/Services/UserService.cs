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

        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserService(CarRentalDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
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

            IQueryable<User> users;

            if (!await _roleManager.RoleExistsAsync(filter.RoleName))
            {
                users = _userManager.Users;
            }
            else
            {
                var userList = await _userManager.GetUsersInRoleAsync(filter.RoleName);
                users = userList.AsQueryable();
            }

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

            var results = users.Select(UserDtoSelector).ToList();

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
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles
                };

                return userDetailsDto;
            }

            return null;
        }

        public async Task DeleteUser(int? userId)
        {
            var reservations = await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            var comments = await _dbContext.Comments
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var ratings = await _dbContext.Ratings
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var user = await _dbContext.Users
                .Include(u => u.Reservations)
                .Include(u => u.Comments)
                .Include(u => u.Ratings)
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            foreach (var item in reservations)
            {
                item.User = null;
                user.Reservations.Remove(item);
                if (item.State == Reservation.ReservationStates.Undecieded)
                {
                    item.State = Reservation.ReservationStates.Cancled;
                }

                if (item.State == Reservation.ReservationStates.Accepted && item.PickUpTime.Date >= DateTime.Now.Date)
                {
                    var car = await _dbContext.Cars.Where(c => c.Id == item.CarId).Include(c => c.Reservations).SingleOrDefaultAsync();
                    if (car != null)
                    {
                        car.Reservations.Remove(item);
                        item.Car = null;
                        item.State = Reservation.ReservationStates.Cancled;
                    }
                }
            }

            foreach (var item in comments)
            {
                item.User = null;
                user.Comments.Remove(item);
            }

            foreach (var item in ratings)
            {
                item.User = null;
                user.Ratings.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task SetCultureInfoAsync(int? id, string cultureInfo)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();

            user.Culture = cultureInfo;

            _dbContext.Attach(user).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
