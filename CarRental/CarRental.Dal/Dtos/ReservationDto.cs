using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Dal.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "PICK_UP_TIME_REQUIRED")]
        [DataType(DataType.DateTime)]
        [Display(Name = "PICK_UP_TIME")]
        public DateTime PickUpTime { get; set; }
        [Required(ErrorMessage = "DROP_OFF_TIME_REQUIRED")]
        [DataType(DataType.DateTime)]
        [Display(Name = "DROP_OFF_TIME")]
        public DateTime DropOffTime { get; set; }
        public int AddressId { get; set; }
        [Display(Name = "ADDRESS")]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "PRICE")]
        public decimal Price { get; set; }

        [Display(Name = "USER")]
        public int? UserId { get; set; }
        [Display(Name = "USER")]
        public string User { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public int VehicleModelId { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public string VehicleType { get; set; }
    }
}
