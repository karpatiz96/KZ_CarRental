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
using CarRental.Bll.Filters;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators, Assistant")]
    public class IndexModel : PageModel
    {
        private readonly ICarService _carService;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ICarService carService, ILogger<IndexModel> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        [BindProperty]
        public PagedResult<CarDto> Cars { get; private set; }

        [BindProperty(SupportsGet = true)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        public string VehicleType { get; set; }

        [BindProperty(SupportsGet = true)]
        [MaxLength(7)]
        public string PlateNumber { get; set; }

        public string TypeSort { get; set; }
        public string PlateSort { get; set; }
        public string ActiveSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string vehicleType, string currentVehicleType, string plateNumber, string currentPlateNumber, int? pageNumber)
        {
            CarFilter filter = new CarFilter();
            CurrentSort = sortOrder;
            PlateSort = string.IsNullOrEmpty(sortOrder) ? "plate_desc" : "";
            TypeSort = sortOrder == "Type" ? "type_desc" : "Type";
            ActiveSort = sortOrder == "Active" ? "active_desc" : "Active";

            if (vehicleType != null || plateNumber != null)
            {
                pageNumber = 0;
            }
            else
            {
                if (string.IsNullOrEmpty(vehicleType))
                    vehicleType = currentVehicleType;

                if (string.IsNullOrEmpty(plateNumber))
                    plateNumber = currentPlateNumber;
            }

            VehicleType = vehicleType;
            PlateNumber = plateNumber;

            filter.PageNumber = pageNumber ?? 0;
            filter.VehicleType = vehicleType;
            filter.PlateNumber = plateNumber;

            switch (sortOrder)
            {
                case "plate_desc": filter.carOrder = CarFilter.CarOrder.PlateNumberDescending;
                    break;
                case "type_desc":
                    filter.carOrder = CarFilter.CarOrder.VehicleTypeDescending;
                    break;
                case "Type":
                    filter.carOrder = CarFilter.CarOrder.VehicleTypeAscending;
                    break;
                case "active_desc":
                    filter.carOrder = CarFilter.CarOrder.ActiveDescending;
                    break;
                case "Active":
                    filter.carOrder = CarFilter.CarOrder.ActiveAscending;
                    break;
                case "": filter.carOrder = CarFilter.CarOrder.PlateNumberAscending;
                    break;
                default: filter.carOrder = CarFilter.CarOrder.PlateNumberAscending;
                    break;
            }

            _logger.LogInformation(LoggingEvents.ListItems, "List Cars");
            Cars = await _carService.GetCars(filter);

            return Page();
        }

    }
}
