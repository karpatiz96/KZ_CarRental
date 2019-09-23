using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Bll.Dtos;
using CarRental.Dal.Entities;
using CarRental.Bll.Filters;
using CarRental.Bll.Logging;
using CarRental.Bll.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static CarRental.Bll.Filters.ReservationListFilter;

namespace CarRental.Web.Pages.Reservations
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly IReservationService _reservationService;

        private readonly ILogger<ListModel> _logger;

        public ListModel(IReservationService reservationService, UserManager<User> userManager, ILogger<ListModel> logger)
        {
            _reservationService = reservationService;
            _userManager = userManager;
            _logger = logger;
        }

        public PagedResult<ReservationListHeader> Reservation { get; set; }

        public string AddressSort { get; set; }
        public string PickUpSort { get; set; }
        public string DropOffSort { get; set; }
        public string VehicleModelSort { get; set; }
        public string StateSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, int? pageNumber)
        {
            ReservationListFilter filter = new ReservationListFilter();

            User user = await _userManager.GetUserAsync(HttpContext.User);

            if(user == null)
            {
                NotFound();
            }

            CurrentSort = sortOrder;
            PickUpSort = string.IsNullOrEmpty(sortOrder) ? "pickup_desc" : "";
            DropOffSort = sortOrder == "DropOff" ? "dropoff_desc" : "DropOff";
            AddressSort = sortOrder == "Address" ? "address_desc" : "Address";
            VehicleModelSort = sortOrder == "VehicleModel" ? "vehiclemodel_desc" : "VehicleModel";
            StateSort = sortOrder == "State" ? "state_desc" : "State";

            filter.PageNumber = pageNumber ?? 0;

            switch (sortOrder)
            {
                case "pickup_desc":
                    filter.reservationOrder = ReservationOrder.PickUpDescending;
                    break;
                case "dropoff_desc":
                    filter.reservationOrder = ReservationOrder.DropOffDescending;
                    break;
                case "DropOff":
                    filter.reservationOrder = ReservationOrder.DropOffAscending;
                    break;
                case "address_desc":
                    filter.reservationOrder = ReservationOrder.AddressDescending;
                    break;
                case "Address":
                    filter.reservationOrder = ReservationOrder.AddressAscending;
                    break;
                case "VehicleModel":
                    filter.reservationOrder = ReservationOrder.VehicleModelAscending;
                    break;
                case "vehiclemodel_desc":
                    filter.reservationOrder = ReservationOrder.VehicleModelDescending;
                    break;
                case "State":
                    filter.reservationOrder = ReservationOrder.StateAscending;
                    break;
                case "state_desc":
                    filter.reservationOrder = ReservationOrder.StateDescending;
                    break;
                default:
                    filter.reservationOrder = ReservationOrder.PickUpAscending;
                    break;
            }

            _logger.LogInformation(LoggingEvents.ListItems, "List Reservation");
            Reservation = await _reservationService.GetReservations(filter, user.Id);
        }

    }
}