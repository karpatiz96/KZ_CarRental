using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Web.Pages.Users
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly ILogger<EditModel> _logger;

        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public EditModel(UserManager<User> userManager,ILogger<EditModel> logger, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public UserEditDto Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get User {ID}", id);

            var user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "User {ID} NOT FOUND", id);
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            Input = new UserEditDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles,
                RoleName = roles.FirstOrDefault()
            };

            ViewData["RoleName"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["RoleName"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
                return Page();
            }

            var user = await _userManager.Users.Where(u => u.Id == Input.Id).FirstOrDefaultAsync();

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            }

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            if (await _roleManager.RoleExistsAsync(Input.RoleName))
            {
                await _userManager.AddToRoleAsync(user, Input.RoleName);
            }

            return RedirectToPage("./Index");
        }
    }
}
