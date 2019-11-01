using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Bll.Dtos;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize]
    public class CancelModel : PageModel
    {
        private readonly IReservationService _reservationService;

        private readonly UserManager<User> _userManager;

        private readonly ILogger<CancelModel> _logger;

        public CancelModel(IReservationService reservationService, UserManager<User> userManager, ILogger<CancelModel> logger)
        {
            _reservationService = reservationService;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public ReservationHeader Reservation { get; set; }

        public async Task<IActionResult> OnGet(int? id)
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

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user.Id != Reservation.UserId.Value)
            {
                return RedirectToPage("./List");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!_reservationService.ReservationExists(Reservation.Id))
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Reservation {ID} NOT FOUND", Reservation.Id);
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user.Id != Reservation.UserId.Value)
            {
                return RedirectToPage("./List");
            }

            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Reservation {ID} with Cancel State", Reservation.Id);
                await _reservationService.CancelReservation(Reservation.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_reservationService.ReservationExists(Reservation.Id))
                {
                    _logger.LogInformation(LoggingEvents.UpdateItemNotFound, "Update Reservation {ID} NOT FOUND", Reservation.Id);
                    return NotFound();
                }
                else
                {
                    return StatusCode(409);
                }
            }

            return RedirectToPage("./List");
        }
    }
}