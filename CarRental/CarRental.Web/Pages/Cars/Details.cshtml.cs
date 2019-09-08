using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Dal.Dtos;
using CarRental.Dal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Dal.Logging;

namespace CarRental.Web.Pages.Cars
{
    [Authorize(Roles = "Administrators")]
    public class DetailsModel : PageModel
    {
        private readonly CarRentalDbContext _context;

        private readonly ICarService _carService;

        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(CarRentalDbContext context, ICarService carService, ILogger<DetailsModel> logger)
        {
            _context = context;
            _carService = carService;
            _logger = logger;
        }

        public CarDto Car { get; set; }

        public bool HasReservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _logger.LogInformation(LoggingEvents.GetItem, "Get Car {ID}", id);
            Car = await _carService.GetCar(id);

            if (Car == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Car {ID} NOT FOUND", id);
                return NotFound();
            }

            HasReservation = await _carService.CarHasReservations(id);

            return Page();
        }
    }
}
