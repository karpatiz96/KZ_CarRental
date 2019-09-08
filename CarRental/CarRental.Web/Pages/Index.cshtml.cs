using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Dal.Dtos;
using CarRental.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRental.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly IVehicleModelService _vehicleModelService;

        public IndexModel(IAddressService addressService, IVehicleModelService vehicleModelService)
        {
            _addressService = addressService;
            _vehicleModelService = vehicleModelService;
        }

        public IEnumerable<AddressDto> Addresses;

        public IEnumerable<VehicleDto> Vehicles;

        public async Task<IActionResult> OnGet()
        {
            Addresses = await _addressService.GetAddresses();
            Vehicles = await _vehicleModelService.GetBestOffers(2);

            return Page();
        }
    }
}
