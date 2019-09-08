using CarRental.Dal.Entities;
using CarRental.Dal.SeedInterfaces;
using CarRental.Dal.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.SeedServices
{
    public class UserSeedService : IUserSeedService
    {
        private readonly UserManager<User> _userManager;

        public UserSeedService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedUserAsync()
        {
            if(!(await _userManager.GetUsersInRoleAsync(Roles.Administrators)).Any())
            {
                var user = new User
                {
                    Email = "admin@carrental.com",
                    Name = "Administrator",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin@carrental.com"
                };

                var createResult = await _userManager.CreateAsync(user, "Asd123+");
                var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Administrators);

                if(!createResult.Succeeded || !addToRoleResult.Succeeded)
                {
                    throw new ApplicationException($"Administrator could not be created: " +
                        $"{string.Join(", ", createResult.Errors.Concat(addToRoleResult.Errors).Select(e => e.Description))}");
                }
            }
        }
    }
}
