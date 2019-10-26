using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators, Assistant")]
    public class DeleteModel : PageModel
    {
        private readonly ICarService _carService;

        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ICarService carService, ILogger<DeleteModel> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        [BindProperty]
        public CarDetailsDto Car { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Car {ID}", id);
            Car = await _carService.GetCarDetailsDto(id);

            if (Car == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Car {ID} NOT FOUND", id);
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

            _logger.LogInformation(LoggingEvents.GetItem, "Get Car {ID}", id);
            Car = await _carService.GetCarDetailsDto(id);

            if(Car != null)
            {
                if (!Car.HasReservation)
                {
                    await _carService.DeleteCar(id);
                    _logger.LogInformation(LoggingEvents.DeleteItem, "Admin Deleted Car {ID}", id);
                }

            }

            return RedirectToPage("./Index");
        }
    }
}
