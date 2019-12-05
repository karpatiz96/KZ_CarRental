using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Bll.IServices;
using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize(Roles = "Administrators, Assistant")]
    public class IndexModel : PageModel
    {
        private readonly CarRentalDbContext _context;

        private readonly IReservationService _reservationService;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(CarRentalDbContext context, IReservationService reservationService, ILogger<IndexModel> logger)
        {
            _context = context;
            _reservationService = reservationService;
            _logger = logger;
        }

        public PagedResult<ReservationIndexHeader> Reservation { get;set; }

        [BindProperty(SupportsGet = true)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        public string VehicleType { get; set; }

        public string AddressSort { get; set; }
        public string PickUpSort { get; set; }
        public string DropOffSort { get; set; }
        public string VehicleModelSort { get; set; }
        public string StateSort { get; set; }
        public string CarSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string vehicleType, string currentVehicleType, int? pageNumber)
        {
            ReservationFilter filter = new ReservationFilter();

            CurrentSort = sortOrder;
            PickUpSort = string.IsNullOrEmpty(sortOrder) ? "pickup_desc" : "";
            DropOffSort = sortOrder == "DropOff" ? "dropoff_desc" : "DropOff";
            AddressSort = sortOrder == "Address" ? "address_desc" : "Address";
            VehicleModelSort = sortOrder == "VehicleModel" ? "vehiclemodel_desc" : "VehicleModel";
            StateSort = sortOrder == "State" ? "state_desc" : "State";
            CarSort = sortOrder == "Car" ? "car_desc" : "Car";

            if (vehicleType != null)
            {
                pageNumber = 0;
            }
            else
            {
                if (string.IsNullOrEmpty(vehicleType))
                    vehicleType = currentVehicleType;
            }

            VehicleType = vehicleType;

            filter.PageNumber = pageNumber ?? 0;
            filter.VehicleType = vehicleType;

            switch (sortOrder)
            {
                case "pickup_desc":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.PickUpDescending;
                    break;
                case "dropoff_desc":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.DropOffDescending;
                    break;
                case "DropOff":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.DropOffAscending;
                    break;
                case "address_desc":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.AddressDescending;
                    break;
                case "Address":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.AddressAscending;
                    break;
                case "VehicleModel":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.VehicleModelAscending;
                    break;
                case "vehiclemodel_desc":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.VehicleModelDescending;
                    break;
                case "State":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.StateAscending;
                    break;
                case "state_desc":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.StateDescending;
                    break;
                case "Car":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.CarAscending;
                    break;
                case "car_desc":
                    filter.reservationOrder = ReservationFilter.ReservationOrder.CarDescending;
                    break;
                default:
                    filter.reservationOrder = ReservationFilter.ReservationOrder.PickUpAscending;
                    break;
            }

            _logger.LogInformation(LoggingEvents.ListItems, "List Reservation for Admin");
            Reservation = await _reservationService.GetReservations(filter);
        }
    }
}
