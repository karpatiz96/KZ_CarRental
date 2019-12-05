using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Dal;
using CarRental.Bll.Dtos;
using CarRental.Dal.Entities;
using CarRental.Bll.Logging;
using CarRental.Bll.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static CarRental.Dal.Entities.Reservation;
using Microsoft.AspNetCore.Identity.UI.Services;
using CarRental.Web.ViewRender;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize(Roles = "Customer")]
    public class CreateConfirmModel : PageModel
    {

        private readonly CarRentalDbContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IReservationService _reservationService;

        private readonly IVehicleModelService _vehicleModelService;

        private readonly IAddressService _addressService;

        private readonly ICarService _carService;

        private readonly ILogger<CreateConfirmModel> _logger;

        private readonly IEmailSender _emailSender;

        private readonly IRazorViewToStringRender _render;

        public CreateConfirmModel(CarRentalDbContext context, UserManager<User> userManager, 
            IReservationService reservationService, IVehicleModelService vehicleModelService, 
            IAddressService addressService, ICarService carService, 
            ILogger<CreateConfirmModel> logger, IEmailSender emailSender,
            IRazorViewToStringRender render)
        {
            _context = context;
            _userManager = userManager;
            _reservationService = reservationService;
            _vehicleModelService = vehicleModelService;
            _addressService = addressService;
            _carService = carService;
            _logger = logger;
            _emailSender = emailSender;
            _render = render;
        }

        public ReservationDto Reservation { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public DateTime PickUpTime { get; set; }
            public DateTime DropOffTime { get; set; }
            public int AddressId { get; set; }
            public int VehicleModelId { get; set; }
        }

        public int CarFound { get; set; }

        public async Task<IActionResult> OnGetAsync(int vehicleId, int addressId, DateTime pickUp, DateTime dropOff)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var vehicle = await _vehicleModelService.GetVehicle(vehicleId);
            var address = await _addressService.GetAddress(addressId);
            int days = (dropOff.Date - pickUp.Date).Days;

            if (user == null || vehicle == null || address == null)
            {
                return NotFound();
            }

            ReservationDto reservation = new ReservationDto
            {
                User = user.UserName,
                UserId = user.Id,
                Address = address.ZipCode + " " + address.City + " " + address.StreetAddress,
                AddressId = address.Id,
                PickUpTime = pickUp,
                DropOffTime = dropOff,
                VehicleModelId = vehicle.Id,
                VehicleType = vehicle.VehicleType,
                Price = days * vehicle.PricePerDay
            };

            Input = new InputModel
            {
                AddressId = address.Id,
                PickUpTime = pickUp,
                DropOffTime = dropOff,
                VehicleModelId = vehicle.Id,
            };

            Reservation = reservation;

            var cars = await _carService.GetCars(reservation.PickUpTime, reservation.DropOffTime, reservation.VehicleModelId);

            CarFound = cars.Count();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var vehicle = await _vehicleModelService.GetVehicle(Input.VehicleModelId);
            var address = await _addressService.GetAddress(Input.AddressId);
            int days = (Input.DropOffTime.Date - Input.PickUpTime.Date).Days;

            if (user == null || vehicle == null || address == null)
            {
                return NotFound();
            }

            if (days < 1)
            {
                return RedirectToPage("./List");
            }

            if (days > 100)
            {
                return RedirectToPage("./List");
            }

            if (Input.PickUpTime.Date < DateTime.Now.Date.AddDays(1))
            {
                return RedirectToPage("./List");
            }

            var cars = await _carService.GetCars(Input.PickUpTime, Input.DropOffTime, Input.VehicleModelId);

            CarFound = cars.Count();

            if(CarFound <= 0)
            {
                return RedirectToPage("./List");
            }

            ReservationDto reservationDto = new ReservationDto
            {
                User = user.UserName,
                UserId = user.Id,
                Address = address.ZipCode + " " + address.City + " " + address.StreetAddress,
                AddressId = address.Id,
                PickUpTime = Input.PickUpTime,
                DropOffTime = Input.DropOffTime,
                VehicleModelId = vehicle.Id,
                VehicleType = vehicle.VehicleType,
                Price = days * vehicle.PricePerDay
            };

            Reservation = reservationDto;

            _logger.LogInformation(LoggingEvents.InsertItem, "Create Reservation");
            await _reservationService.CreateReservation(reservationDto);

            var model = new EmailReservationDto
            {
                UserName = user.Name ?? user.Email,
                Email = user.Email,
                VehicleType = reservationDto.VehicleType,
                Address = reservationDto.Address,
                PickUpTime = reservationDto.PickUpTime,
                DropOffTime = reservationDto.DropOffTime,
                Price = reservationDto.Price,
                State = ReservationStates.Undecieded
            };

            const string view = "/Views/Emails/ReservationEmail";
            var message = await _render.RenderViewToStringAsync($"{view}Html.cshtml", model);
            await _emailSender.SendEmailAsync(user.Email, "Reservation", message);

            return RedirectToPage("./List");
        }
    }
}