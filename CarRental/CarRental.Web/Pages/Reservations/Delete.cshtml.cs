using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize(Roles = "Administrators")]
    public class DeleteModel : PageModel
    {
        private readonly CarRentalDbContext _context;

        private readonly IReservationService _reservationService;

        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(CarRentalDbContext context, IReservationService reservationService, ILogger<DeleteModel> logger)
        {
            _context = context;
            _reservationService = reservationService;
            _logger = logger;
        }

        public ReservationHeader Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Reservation {ID}", id);
            Reservation = await _reservationService.GetReservation(id);

            if (Reservation == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Reservation {ID} NOT FOUND", id);
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

            _logger.LogInformation(LoggingEvents.GetItem, "Get Reservation {ID}", id);
            Reservation = await _reservationService.GetReservation(id);

            if (Reservation != null)
            {
                await _reservationService.DeleteReservation(id);
                _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Reservation {ID}", id);
            }

            return RedirectToPage("./Index");
        }
    }
}
