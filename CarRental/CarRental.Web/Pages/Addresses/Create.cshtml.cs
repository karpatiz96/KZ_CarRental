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
using CarRental.Bll.Dtos;
using System.ComponentModel.DataAnnotations;
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Addresses
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IAddressService addressService, ILogger<CreateModel> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AddressInputDto Address { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Admin created a new Address");
            await _addressService.CreateAddress(Address);

            return RedirectToPage("./Index");
        }
    }
}