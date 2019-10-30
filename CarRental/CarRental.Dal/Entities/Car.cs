using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        public bool Active { get; set; }

        public int VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
