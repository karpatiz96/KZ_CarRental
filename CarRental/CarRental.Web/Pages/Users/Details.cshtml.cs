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
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Dtos;
using CarRental.Bll.Logging;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Web.Pages.Users
{
    [Authorize(Roles = "Administrators")]
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;

        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IUserService userService, ILogger<DetailsModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public UserDetailsDto UserDto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get User {ID}", id);
            UserDto = await _userService.GetUserDetails(id);

            if (User == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get User {ID} NOT FOUND", id);
                return NotFound();
            }

            return Page();
        }
    }
}
