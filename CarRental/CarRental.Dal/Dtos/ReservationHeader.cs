using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static CarRental.Dal.Entities.Reservation;

namespace CarRental.Dal.Dtos
{
    public class ReservationHeader
    {
        public int Id { get; set; }
        [Display(Name = "PICK_UP_TIME")]
        public DateTime PickUpTime { get; set; }
        [Display(Name = "DROP_OFF_TIME")]
        public DateTime DropOffTime { get; set; }
        public int AddressId { get; set; }
        [Display(Name = "ADDRESS")]
        public string Address { get; set; }
        [Display(Name = "PRICE")]
        public decimal Price { get; set; }

        [Display(Name = "USER")]
        public int? UserId { get; set; }
        [Display(Name = "USER")]
        public string User { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public int? VehicleModelId { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public string VehicleType { get; set; }
        [Display(Name = "CAR")]
        public int? CarId { get; set; }
        [Display(Name = "CAR")]
        public string Car { get; set; }
        [Display(Name = "STATE")]
        public ReservationStates State { get; set; }
    }
}
