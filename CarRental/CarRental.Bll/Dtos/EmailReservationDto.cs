using System;
using System.ComponentModel.DataAnnotations;
using static CarRental.Dal.Entities.Reservation;

namespace CarRental.Bll.Dtos
{
    public class EmailReservationDto
    {
        [Display(Name = "USER")]
        public string UserName { get; set; }
        [Display(Name = "EMAIL")]
        public string Email { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public string VehicleType { get; set; }
        [Display(Name = "PICK_UP_TIME")]
        public DateTime PickUpTime { get; set; }
        [Display(Name = "DROP_OFF_TIME")]
        public DateTime DropOffTime { get; set; }
        [Display(Name = "ADDRESS")]
        public string Address { get; set; }
        [Display(Name = "PRICE")]
        public decimal Price { get; set; }
        [Display(Name = "STATE")]
        public ReservationStates State { get; set; }
    }
}
