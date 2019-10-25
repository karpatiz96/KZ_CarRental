using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using CarRental.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarRental.Web.Pages.VehicleModels
{
    [Authorize(Roles = "Administrators, Assisstant")]
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;

        private readonly IVehicleModelService _vehicleModelService;

        public DeleteModel(IVehicleModelService vehicleModelService, ILogger<DeleteModel> logger)
        {
            _vehicleModelService = vehicleModelService;
            _logger = logger;
        }

        [BindProperty]
        public VehicleModelDeleteDto VehicleModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem,"Get VehicleModel {ID}", id);
            VehicleModel = await _vehicleModelService.GetVehicleModelDelete(id);

            if (VehicleModel == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "VehicleModel {ID} NOT FOUND", id);
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get VehicleModel {ID}", id);

            VehicleModel = await _vehicleModelService.GetVehicleModelDelete(id);

            if (VehicleModel != null)
            {
                if (VehicleModel.HasReservation)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    await _vehicleModelService.DeleteVehicle(id);
                    _logger.LogInformation(LoggingEvents.DeleteItem, "Admin deleted VehicleModel {ID}", id);
                }

            }

            return RedirectToPage("./Index");
        }
    }
}
