using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class ReservationInputDto
    {
        [Required(ErrorMessage = "PICK_UP_TIME_REQUIRED")]
        [DataType(DataType.DateTime)]
        [Display(Name = "PICK_UP_TIME")]
        public DateTime? PickUpTime { get; set; }
        [Required(ErrorMessage = "DROP_OFF_TIME_REQUIRED")]
        [DataType(DataType.DateTime)]
        [Display(Name = "DROP_OFF_TIME")]
        public DateTime? DropOffTime { get; set; }
        [Display(Name = "ADDRESS")]
        public int AddressId { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public int VehicleModelId { get; set; }
    }
}
