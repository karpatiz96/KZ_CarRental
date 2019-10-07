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
    [Authorize(Roles = "Administrators")]
    public class CreateModel : PageModel
    {
        private readonly IVehicleModelService _vehicleModelService;

        private readonly CarRentalDbContext _dbContext;

        private readonly ILogger<CreateModel> _logger;

        public CreateModel(CarRentalDbContext dbContext,IVehicleModelService vehicleModelService, ILogger<CreateModel> logger)
        {
            _dbContext = dbContext;
            _vehicleModelService = vehicleModelService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public VehicleModelInputDto VehicleModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _vehicleModelService.CreateVehicleModel(VehicleModel);
            _logger.LogInformation(LoggingEvents.InsertItem, "Admin created new VehicleModel");

            return RedirectToPage("./Index");
        }
    }
}