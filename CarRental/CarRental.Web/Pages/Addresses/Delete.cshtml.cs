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

namespace CarRental.Web.Pages.Addresses
{
    [Authorize(Roles = "Administrators")]
    public class DeleteModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IAddressService addressService, ILogger<DeleteModel> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [BindProperty]
        public AddressDto Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Address {ID}", id);
            Address = await _addressService.GetAddress(id);

            if (Address == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Address {ID} NOT FOUND", id);
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

            _logger.LogInformation(LoggingEvents.GetItem, "Get Address {ID}", id);
            Address = await _addressService.GetAddress(id);

            if (Address == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Address {ID} NOT FOUND", Address.Id);
                return NotFound();
            }

            if (!_addressService.AddressHasReservations(id))
            {
                try
                {
                    await _addressService.DeleteAddress(id);
                    _logger.LogInformation(LoggingEvents.DeleteItem, "Admin Deleted Address {ID}", id);
                }
                catch(DbUpdateException)
                {

                }
            }
            

            return RedirectToPage("./Index");
        }
    }
}
