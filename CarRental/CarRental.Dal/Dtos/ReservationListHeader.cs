using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static CarRental.Dal.Entities.Reservation;

namespace CarRental.Dal.Dtos
{
    public class ReservationListHeader
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

        [Display(Name = "VEHICLE_MODEL")]
        public int? VehicleModelId { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public string VehicleType { get; set; }
        [Display(Name = "STATE")]
        public ReservationStates State { get; set; }
    }
}
