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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, ILogger<CreateModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["RoleName"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return Page();
        }

        [BindProperty]
        public UserInputDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["RoleName"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");

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

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Administrator created a new account with password.");

                if(await _roleManager.RoleExistsAsync(Input.RoleName))
                {
                    var addToRole = await _userManager.AddToRoleAsync(user, Input.RoleName);

                    if (addToRole.Succeeded)
                    {
                        _logger.LogInformation($"User {0} added to role {1}.", user.Id, Input.RoleName);
                    }
                }

                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}