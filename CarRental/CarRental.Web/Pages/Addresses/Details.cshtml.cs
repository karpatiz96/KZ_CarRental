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
using Microsoft.Extensions.Configuration;

namespace CarRental.Web.Pages.Addresses
{
    [Authorize(Roles = "Administrators, Assistant")]
    public class DetailsModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly ILogger<DetailsModel> _logger;

        private readonly IConfiguration _configuration;

        public DetailsModel(IAddressService addressService, ILogger<DetailsModel> logger, IConfiguration configuration)
        {
            _addressService = addressService;
            _logger = logger;
            _configuration = configuration;
        }

        public AddressDetailsDto Address { get; set; }

        public String MapKey { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Address {ID}", id);
            Address = await _addressService.GetAddressDetails(id);
            MapKey = _configuration.GetValue<String>("AzureMapSubscriptionKey");

            if (Address == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Address {ID} NOT FOUND", id);
                return NotFound();
            }

            return Page();
        }
    }
}
