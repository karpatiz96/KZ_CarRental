using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using System.ComponentModel.DataAnnotations;
using CarRental.Bll.IServices;
using CarRental.Bll.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        private readonly ICarService _carService;

        private readonly IReservationService _reservationService;

        private readonly ILogger<EditModel> _logger;

        public EditModel(ICarService carService, IReservationService reservationService, ILogger<EditModel> logger)
        {
            _carService = carService;
            _reservationService = reservationService;
            _logger = logger;
        }


        [BindProperty]
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

            var cars = await _carService.GetCars(Reservation.PickUpTime, Reservation.DropOffTime, Reservation.VehicleModelId);
            ViewData["CarId"] = new SelectList(cars, "Id", "PlateNumber");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var cars = await _carService.GetCars(Reservation.PickUpTime, Reservation.DropOffTime, Reservation.VehicleModelId);
                ViewData["CarId"] = new SelectList(cars, "Id", "PlateNumber");
                return Page();
            }

            _logger.LogInformation(LoggingEvents.GetItem, "Get Car {ID}", Reservation.CarId);
            var car = await _carService.GetCar(Reservation.CarId);

            if(car == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Car {ID} NOT FOUND", Reservation.CarId);
                return NotFound();
            }

            if (!_reservationService.ReservationExists(Reservation.Id))
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Reservation {ID} NOT FOUND", Reservation.Id);
                return NotFound();
            }

            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Reservation {ID} with Car {ID}", Reservation.Id, Reservation.CarId);
                await _reservationService.EditReservation(Reservation.Id, car.Id);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!_reservationService.ReservationExists(Reservation.Id))
                {
                    _logger.LogInformation(LoggingEvents.UpdateItemNotFound, "Update Reservation {ID} NOT FOUND", Reservation.Id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            if (!ModelState.IsValid)
            {
                var cars = await _carService.GetCars(Reservation.PickUpTime, Reservation.DropOffTime, Reservation.VehicleModelId);
                ViewData["CarId"] = new SelectList(cars, "Id", "PlateNumber");
                return Page();
            }

            if (!_reservationService.ReservationExists(Reservation.Id))
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, "Get Reservation {ID} NOT FOUND", Reservation.Id);
                return NotFound();
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
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

    }
}
