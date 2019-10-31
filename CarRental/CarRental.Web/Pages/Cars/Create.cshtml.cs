using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators, Assistant")]
    public class CreateModel : PageModel
    {
        private readonly ICarService _carService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly IAddressService _addressService;

        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ICarService carService, IVehicleModelService vehicleModelService, IAddressService addressService, ILogger<CreateModel> logger)
        {
            _carService = carService;
            _vehicleModelService = vehicleModelService;
            _addressService = addressService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["VehicleModelId"] = new SelectList(await _vehicleModelService.GetVehicleModels(), "Id", "VehicleType");
            ViewData["AddressId"] = new SelectList(await _addressService.GetAddresses(), "Id", "FullAddress");
            return Page();
        }

        [BindProperty]
        public CarDto Car { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["VehicleModelId"] = new SelectList(await _vehicleModelService.GetVehicleModels(), "Id", "VehicleType");
                ViewData["AddressId"] = new SelectList(await _addressService.GetAddresses(), "Id", "FullAddress");
                return Page();
            }

            _logger.LogInformation(LoggingEvents.InsertItem, "Admin created a new Car");
            await _carService.CreateCar(Car);

            return RedirectToPage("./Index");
        }
    }
}