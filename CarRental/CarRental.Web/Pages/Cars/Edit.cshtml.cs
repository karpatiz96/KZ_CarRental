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

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        private readonly CarRentalDbContext _context;

        private readonly ICarService _carService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly ILogger<EditModel> _logger;

        public EditModel(CarRentalDbContext context, ICarService carService, IVehicleModelService vehicleModelService, ILogger<EditModel> logger)
        {
            _context = context;
            _carService = carService;
            _vehicleModelService = vehicleModelService;
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

            ViewData["VehicleModelId"] = new SelectList(_vehicleModelService.GetActiveVehicles(), "Id", "VehicleType", Car.VehicleModelId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["VehicleModelId"] = new SelectList(_vehicleModelService.GetActiveVehicles(), "Id", "VehicleType", Car.VehicleModelId);
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
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
