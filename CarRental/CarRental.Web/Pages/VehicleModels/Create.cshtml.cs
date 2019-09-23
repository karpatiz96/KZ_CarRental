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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.VehicleModels
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : PageModel
    {
        private readonly IVehicleModelService _vehicleModelService;

        private readonly CarRentalDbContext _dbContext;

        private readonly IHostingEnvironment _hosting;

        private readonly ILogger<CreateModel> _logger;

        public CreateModel(CarRentalDbContext dbContext,IVehicleModelService vehicleModelService, IHostingEnvironment hosting, ILogger<CreateModel> logger)
        {
            _dbContext = dbContext;
            _vehicleModelService = vehicleModelService;
            _hosting = hosting;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InputModel VehicleModel { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "VEHICLE_MODEL_REQUIRED")]
            [Display(Name = "VEHICLE_MODEL")]
            public string VehicleType { get; set; }

            [Required(ErrorMessage = "PRICE_PER_DAY_REQUIRED")]
            [DataType(DataType.Currency)]
            [Display(Name = "PRICE_PER_DAY")]
            [Range(0, int.MaxValue, ErrorMessage = "PRICE_PER_DAY_VALUE")]
            public decimal? PricePerDay { get; set; }

            [Required(ErrorMessage = "PICTURE_REQUIRED")]
            [Display(Name = "PICTURE")]
            public IFormFile Picture { get; set; }

            [Required(ErrorMessage = "NUMBER_OF_DOORS_REQUIRED")]
            [Range(0, int.MaxValue, ErrorMessage = "NUMBER_OF_DOORS_VALUE")]
            [Display(Name = "NUMBER_OF_DOORS")]
            public int? NumberOfDoors { get; set; }

            [Required(ErrorMessage = "NUMBER_OF_SEATS_REQUIRED")]
            [Range(0, int.MaxValue, ErrorMessage = "NUMBER_OF_SEATS_VALUE")]
            [Display(Name = "NUMBER_OF_SEATS")]
            public int? NumberOfSeats { get; set; }

            [Display(Name = "AUTOMATIC")]
            public bool Automatic { get; set; }

            [Display(Name = "AIR_CONDITIONED")]
            public bool AirConditioning { get; set; }

            [Display(Name = "ACTIVE")]
            public bool Active { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            VehicleModelDto vehicle = new VehicleModelDto
            {
               VehicleType = VehicleModel.VehicleType,
               PricePerDay = VehicleModel.PricePerDay.Value,
               NumberOfDoors = VehicleModel.NumberOfDoors.Value,
               NumberOfSeats = VehicleModel.NumberOfSeats.Value,
               Active = VehicleModel.Active,
               AirConditioning = VehicleModel.AirConditioning,
               Automatic = VehicleModel.Automatic,
               VehicleUrl = string.Empty
            };

            await _vehicleModelService.CreateVehicle(vehicle, VehicleModel.Picture);
            _logger.LogInformation(LoggingEvents.InsertItem, "Admin created new VehicleModel");

            return RedirectToPage("./Index");
        }
    }
}