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
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using CarRental.Bll.Logging;
using CarRental.Bll.Dtos;

namespace CarRental.Web.Pages.Addresses
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly ILogger<EditModel> _logger;

        public EditModel(IAddressService addressService, ILogger<EditModel> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [BindProperty]
        public AddressInputDto Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Address {ID}", id);
            var address = await _addressService.GetAddress(id.Value);

            if (address == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Address {ID} NOT FOUND", id);
                return NotFound();
            }

            Address = new AddressInputDto
            {
                Id = address.Id,
                City = address.City,
                ZipCode = address.ZipCode,
                StreetAddress = address.StreetAddress
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Address == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Address {ID}", Address.Id);
            var address = await _addressService.GetAddress(Address.Id);

            if (address == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Address {ID} NOT FOUND", address.Id);
                return NotFound();
            }

            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Admin updated Address {ID}", address.Id);
                await _addressService.EditAddress(Address);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_addressService.AddressExists(address.Id))
                {
                    _logger.LogInformation(LoggingEvents.UpdateItemNotFound, "Updated Address {ID} NOT FOUND", address.Id);
                    return NotFound();
                }
                else
                {
                    return StatusCode(409);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
