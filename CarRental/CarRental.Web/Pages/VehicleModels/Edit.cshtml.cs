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
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.VehicleModels
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        private readonly CarRentalDbContext _context;

        private readonly ILogger<EditModel> _logger;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly IHostingEnvironment _hosting;

        public EditModel(CarRentalDbContext context, IHostingEnvironment hosting, IVehicleModelService vehicleModelService, ILogger<EditModel> logger)
        {
            _context = context;
            _vehicleModelService = vehicleModelService;
            _hosting = hosting;
            _logger = logger;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get VehicleModel {ID}", id);
            var vehicle = await _vehicleModelService.GetVehicle(id);

            VehicleModel = new InputModel
            {
                Id = vehicle.Id,
                VehicleType = vehicle.VehicleType,
                PricePerDay = vehicle.PricePerDay,
                NumberOfDoors = vehicle.NumberOfDoors,
                NumberOfSeats = vehicle.NumberOfSeats,
                Active = vehicle.Active,
                AirConditioning = vehicle.AirConditioning,
                Automatic = vehicle.Automatic
            };

            if (VehicleModel == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "VehicleModel {ID} NOT FOUND", id);
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get VehicleModel {ID}", VehicleModel.Id);
            var vehicleModel = await _vehicleModelService.GetVehicle(VehicleModel.Id);

            if(vehicleModel == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "VehicleModel {ID} NOT FOUND", vehicleModel.Id);
                return NotFound();
            }

            vehicleModel.VehicleType = VehicleModel.VehicleType;
            vehicleModel.PricePerDay = VehicleModel.PricePerDay.Value;
            vehicleModel.NumberOfDoors = VehicleModel.NumberOfDoors.Value;
            vehicleModel.NumberOfSeats = VehicleModel.NumberOfSeats.Value;
            vehicleModel.Active = VehicleModel.Active;
            vehicleModel.AirConditioning = VehicleModel.AirConditioning;
            vehicleModel.Automatic = VehicleModel.Automatic;

            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update VehicleModel {ID}", vehicleModel.Id);
                await _vehicleModelService.EditVehicle(vehicleModel, VehicleModel.Picture);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_vehicleModelService.VehicleModelExists(vehicleModel.Id))
                {
                    _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "VehicleModel {ID} NOT FOUND", vehicleModel.Id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        /*private bool VehicleModelExists(int id)
        {
            return _context.VehicleModels.Any(e => e.Id == id);
        }*/
    }
}
