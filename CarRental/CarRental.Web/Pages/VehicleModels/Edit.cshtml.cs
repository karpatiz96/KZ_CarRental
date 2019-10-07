using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarRental.Web.Pages.VehicleModels
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;

        private readonly IVehicleModelService _vehicleModelService;

        public EditModel(IVehicleModelService vehicleModelService, ILogger<EditModel> logger)
        {
            _vehicleModelService = vehicleModelService;
            _logger = logger;
        }

        [BindProperty]
        public VehicleModelEditDto VehicleModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get VehicleModel {ID}", id);
            var vehicle = await _vehicleModelService.GetVehicle(id);

            VehicleModel = new VehicleModelEditDto
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
    }
}
