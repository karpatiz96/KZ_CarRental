using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRental.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly UserManager<User> UserManager;

        private readonly IReservationService _reservationService;

        public IndexModel(IAddressService addressService, IVehicleModelService vehicleModelService, IReservationService reservationService, UserManager<User> userManager)
        {
            _addressService = addressService;
            _vehicleModelService = vehicleModelService;
            _reservationService = reservationService;
            UserManager = userManager;
        }

        public IEnumerable<AddressDto> Addresses;

        public IEnumerable<VehicleDto> Vehicles;

        public IEnumerable<ReservationListHeader> Reservations;

        public async Task<IActionResult> OnGet()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                Reservations = await _reservationService.GetReservationListHeaders(user.Id);
            }

            Addresses = await _addressService.GetAddresses();
            Vehicles = await _vehicleModelService.GetBestOffers(2);

            return Page();
        }
    }
}
