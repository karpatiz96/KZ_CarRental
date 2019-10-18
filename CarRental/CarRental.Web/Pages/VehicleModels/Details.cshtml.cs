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

        private readonly IRatingService _ratingService;

        private UserManager<User> UserManager;

        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IVehicleModelService vehicleModelService, ICommentService commentService, 
            IRatingService ratingService, UserManager<User> userManager, ILogger<DetailsModel> logger)
        {
            _vehicleModelService = vehicleModelService;
            _commentService = commentService;
            _ratingService = ratingService;
            UserManager = userManager;
            _logger = logger;
        }

        public VehicleModelDetailsDto VehicleModel { get; set; }

        public IEnumerable<CommentDto> Comments { get; private set; }

        public bool IsRated { get; private set; }

        [BindProperty]
        public RatingDto Rating { get; set; }

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

            Comments = await _commentService.GetComments();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    IsRated = await _ratingService.IsRated(VehicleModel.Id, user.Id);

                    if (!IsRated)
                    {
                        Rating = new RatingDto
                        {
                            VehicleModelId = VehicleModel.Id
                        };
                    }
                }
            }

            return Page();
        }        public async Task<IActionResult> OnPostRating()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await UserManager.GetUserAsync(HttpContext.User);

            await _ratingService.PostRating(Rating.Rating, Rating.VehicleModelId, user.Id);

            return RedirectToPage("./Details", new { id = Rating.VehicleModelId } );
        }
    }
}
