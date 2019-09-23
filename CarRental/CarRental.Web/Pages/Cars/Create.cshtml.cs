using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Bll.IServices;
using CarRental.Bll.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : PageModel
    {
        private readonly ICarService _carService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ICarService carService, IVehicleModelService vehicleModelService, ILogger<CreateModel> logger)
        {
            _carService = carService;
            _vehicleModelService = vehicleModelService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            ViewData["VehicleModelId"] = new SelectList(_vehicleModelService.GetVehicles(), "Id", "VehicleType");
            return Page();
        }

        [BindProperty]
        public CarDto Car { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["VehicleModelId"] = new SelectList(_vehicleModelService.GetVehicles(), "Id", "VehicleType");
                return Page();
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Admin created a new Car");
            await _carService.CreateCar(Car);

            return RedirectToPage("./Index");
        }
    }
}