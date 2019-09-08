using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class Reservation
    {
        public enum ReservationStates
        {
            Accepted = 0,
            Cancled = 1,
            Undecieded = 2
        }

        public int Id { get; set; }
        [Required]
        public DateTime PickUpTime { get; set; }
        [Required]
        public DateTime DropOffTime { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public ReservationStates State { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        public int? VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; }
        public int? CarId { get; set; } 
        public Car Car { get; set; }

    }
}
