using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Users
{
    [Authorize(Roles = "Administrators")]
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        private readonly ILogger<DeleteModel> _logger;

        private readonly UserManager<User> _userManager;

        public DeleteModel(IUserService userService, ILogger<DeleteModel> logger, UserManager<User> userManager)
        {
            _userService = userService;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public UserDetailsDto UserDto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get User {ID}", id);
            UserDto = await _userService.GetUserDetails(id);

            if (UserDto == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get User {ID} NOT FOUND", id);
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get User {ID}", id);
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get User {ID} NOT FOUND", id);
                return NotFound();
            }

            await _userService.DeleteUser(user.Id);

            var result = await _userManager.DeleteAsync(user);

            var userId = await _userManager.GetUserIdAsync(user);

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return RedirectToPage("./Index");
        }
    }
}
