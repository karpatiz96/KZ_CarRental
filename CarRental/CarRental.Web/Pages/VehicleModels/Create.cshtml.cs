using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using CarRental.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        public VehicleModelInputDto VehicleModel { get; set; }

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