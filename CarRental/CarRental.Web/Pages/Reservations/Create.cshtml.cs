using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using CarRental.Bll.IServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using CarRental.Bll.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using CarRental.Web.Resources;
using System.Reflection;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize(Roles = "Customer")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly IReservationService _reservationService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly IAddressService _addressService;

        private readonly ILogger<CreateModel> _logger;

        private readonly IStringLocalizer _localizer;

        public CreateModel(UserManager<User> userManager, IReservationService reservationService, IVehicleModelService vehicleModelService, IAddressService addressService, ILogger<CreateModel> logger, IStringLocalizerFactory factory)
        {
            _userManager = userManager;
            _reservationService = reservationService;
            _vehicleModelService = vehicleModelService;
            _addressService = addressService;
            _logger = logger;

            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("IdentityResource", assemblyName.Name);
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            ViewData["AddressId"] = new SelectList(await _addressService.GetAddresses(), "Id", "FullAddress");
            ViewData["VehicleModelId"] = new SelectList(await _vehicleModelService.GetActiveVehicleModels(), "Id", "VehicleType", id);
            return Page();
        }

        [BindProperty]
        public ReservationInputDto Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["AddressId"] = new SelectList(await _addressService.GetAddresses(), "Id", "FullAddress");
            ViewData["VehicleModelId"] = new SelectList(await _vehicleModelService.GetActiveVehicleModels(), "Id", "VehicleType");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            int days = (Input.DropOffTime.Value.Date - Input.PickUpTime.Value.Date).Days;

            if(days < 1)
            {
                ModelState.AddModelError("", _localizer["RESERVE_MIN"]);
                return Page();
            }

            if(days > 100)
            {
                ModelState.AddModelError("", _localizer["RESERVE_MAX"]);
                return Page();
            }

            if(Input.PickUpTime.Value.Date < DateTime.Now.Date.AddDays(1))
            {
                ModelState.AddModelError("", _localizer["RESERVE_TODAY"]);
                return Page();
            }

            var vehicle = await _vehicleModelService.GetVehicle(Input.VehicleModelId);
            var address = await _addressService.GetAddress(Input.AddressId);

            if(vehicle == null || address == null)
            {
                ModelState.AddModelError("", _localizer["RESERVE_TIME_PLACE"]);
                return Page();
            }

            return RedirectToPage("./CreateConfirm", new { vehicleId = vehicle.Id , addressId = address.Id, pickup = Input.PickUpTime.Value, dropoff = Input.DropOffTime.Value});
        }
    }
}