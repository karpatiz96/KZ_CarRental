using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Bll.IServices;
using CarRental.Bll.Dtos;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.VehicleModels
{
    public class DetailsModel : PageModel
    {
        private readonly IVehicleModelService _vehicleModelService;

        private readonly ILogger<DetailsModel> _logger;

        private readonly ICarService _carService;

        public DetailsModel(IVehicleModelService vehicleModelService, ICarService carService, ILogger<DetailsModel> logger)
        {
            _vehicleModelService = vehicleModelService;
            _carService = carService;
            _logger = logger;
        }

        public VehicleModelDto VehicleModel { get; set; }

        public IEnumerable<CarDto> Cars { get; set; }

        public int CarFound { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get VehicleModel {ID}", id);
            VehicleModel = await _vehicleModelService.GetVehicle(id.Value);

            if (VehicleModel == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "VehicleModel {ID} NOT FOUND", id);
                return NotFound();
            }

            Cars = await _carService.GetCarList(VehicleModel.Id);

            CarFound = Cars.Count();
            return Page();
        }
    }
}
