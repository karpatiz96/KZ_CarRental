using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class CarDetailsDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "PLATE_NUMBER_REQUIRED")]
        [Display(Name = "PLATE_NUMBER")]
        [StringLength(7, ErrorMessage = "PLATE_NUMBER_LENGTH")]
        [RegularExpression("^[A-Z]{3}[-][0-9]{3}$", ErrorMessage = "PLATE_NUMBER_LOOK")]
        public string PlateNumber { get; set; }
        [Required]
        [Display(Name = "ACTIVE")]
        public bool Active { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public int VehicleModelId { get; set; }
        [Display(Name = "VEHICLE_MODEL")]
        public string VehicleType { get; set; }
        [Display(Name = "CURRENT_ADDRESS")]
        public int? AddressId { get; set; }
        [Display(Name = "CURRENT_ADDRESS")]
        public string Address { get; set; }
        public bool HasReservation { get; set; }
    }
}
