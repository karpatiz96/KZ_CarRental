using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using CarRental.Web.Resources;
using System.Reflection;
using CarRental.Dal.Users;
using Microsoft.EntityFrameworkCore;
using CarRental.Bll.Dtos;

namespace CarRental.Web.Pages.Users
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> UserManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, ILogger<CreateModel> logger)
        {
            UserManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public UserInputDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                Name = Input.Name,
                UserName = Input.Email,
                Email = Input.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await UserManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                if (Input.RoleId != null)
                {
                    var role = await _roleManager.Roles.Where(r => r.Id == Input.RoleId).FirstOrDefaultAsync();
                    if (role != null)
                    {
                        var addToRole = await UserManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}