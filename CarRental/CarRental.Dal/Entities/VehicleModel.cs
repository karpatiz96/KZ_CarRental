using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class VehicleModel
    {
        public int Id { get; set; }
        [Required]
        public string VehicleType { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }
        [Required]
        public string VehicleUrl { get; set; }        
        public int NumberOfDoors { get; set; }        
        public int NumberOfSeats { get; set; }        
        public bool Automatic { get; set; }        
        public bool AirConditioning { get; set; }
        public bool Active { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
