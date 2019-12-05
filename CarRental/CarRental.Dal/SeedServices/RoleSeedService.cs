using CarRental.Dal.SeedInterfaces;
using CarRental.Dal.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Dal.SeedServices
{
    public class RoleSeedService : IRoleSeedService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleSeedService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRoleAsync()
        {
            if(!await _roleManager.RoleExistsAsync(Roles.Administrators))
            {
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Administrators });
            }

            if(!await _roleManager.RoleExistsAsync(Roles.Assistant))
            {
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Assistant });
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Customer))
            {
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Customer });
            }
        }
    }
}
