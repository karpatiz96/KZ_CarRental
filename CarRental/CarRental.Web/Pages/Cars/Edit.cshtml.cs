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
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;
using System.Net;

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators, Assistant")]
    public class EditModel : PageModel
    {
        private readonly ICarService _carService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly IAddressService _addressService;

        private readonly ILogger<EditModel> _logger;

        public EditModel(ICarService carService, IVehicleModelService vehicleModelService, IAddressService addressService, ILogger<EditModel> logger)
        {
            _carService = carService;
            _vehicleModelService = vehicleModelService;
            _addressService = addressService;
            _logger = logger;
        }

        [BindProperty]
        public CarDto Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Car {ID}", id);
            Car = await _carService.GetCar(id);

            if (Car == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Car {ID} NOT FOUND", id);
                return NotFound();
            }

            ViewData["VehicleModelId"] = new SelectList(await _vehicleModelService.GetActiveVehicleModels(), "Id", "VehicleType", Car.VehicleModelId);
            ViewData["AddressId"] = new SelectList(await _addressService.GetAddresses(), "Id", "FullAddress");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["VehicleModelId"] = new SelectList(await _vehicleModelService.GetActiveVehicleModels(), "Id", "VehicleType", Car.VehicleModelId);
                ViewData["AddressId"] = new SelectList(await _addressService.GetAddresses(), "Id", "FullAddress");
                return Page();
            }

            if(Car == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Car {ID}", Car.Id);
            var car = await _carService.GetCar(Car.Id);

            if(car == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Car {ID} NOT FOUND", car.Id);
                return NotFound();
            }

            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Admin updated Car {ID}", car.Id);
                await _carService.EditCar(Car);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_carService.CarExists(car.Id))
                {
                    _logger.LogInformation(LoggingEvents.UpdateItemNotFound, "Updated Car {ID} NOT FOUND", car.Id);
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
