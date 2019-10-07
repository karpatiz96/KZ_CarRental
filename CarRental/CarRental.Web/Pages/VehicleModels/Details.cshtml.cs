using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Web.Pages.VehicleModels
{
    public class DetailsModel : PageModel
    {
        private readonly IVehicleModelService _vehicleModelService;

        private readonly ICommentService _commentService;

        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IVehicleModelService vehicleModelService, ICommentService commentService, ILogger<DetailsModel> logger)
        {
            _vehicleModelService = vehicleModelService;
            _commentService = commentService;
            _logger = logger;
        }

        public VehicleModelDetailsDto VehicleModel { get; set; }

        public IEnumerable<CommentDto> Comments { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get VehicleModel {ID}", id);
            VehicleModel = await _vehicleModelService.GetVehicleModel(id.Value);

            if (VehicleModel == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "VehicleModel {ID} NOT FOUND", id);
                return NotFound();
            }

            Comments = _commentService.GetComments();

            return Page();
        }
    }
}
