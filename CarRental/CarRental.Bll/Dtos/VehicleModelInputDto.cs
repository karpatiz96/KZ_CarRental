using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class VehicleModelInputDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "VEHICLE_MODEL_REQUIRED")]
        [Display(Name = "VEHICLE_MODEL")]
        public string VehicleType { get; set; }

        [Required(ErrorMessage = "PRICE_PER_DAY_REQUIRED")]
        [DataType(DataType.Currency)]
        [Display(Name = "PRICE_PER_DAY")]
        [Range(0, int.MaxValue, ErrorMessage = "PRICE_PER_DAY_VALUE")]
        public decimal? PricePerDay { get; set; }

        [Required(ErrorMessage = "PICTURE_REQUIRED")]
        [Display(Name = "PICTURE")]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = "NUMBER_OF_DOORS_REQUIRED")]
        [Range(0, int.MaxValue, ErrorMessage = "NUMBER_OF_DOORS_VALUE")]
        [Display(Name = "NUMBER_OF_DOORS")]
        public int? NumberOfDoors { get; set; }

        [Required(ErrorMessage = "NUMBER_OF_SEATS_REQUIRED")]
        [Range(0, int.MaxValue, ErrorMessage = "NUMBER_OF_SEATS_VALUE")]
        [Display(Name = "NUMBER_OF_SEATS")]
        public int? NumberOfSeats { get; set; }

        [Display(Name = "AUTOMATIC")]
        public bool Automatic { get; set; }

        [Display(Name = "AIR_CONDITIONED")]
        public bool AirConditioning { get; set; }

        [Display(Name = "ACTIVE")]
        public bool Active { get; set; }
    }
}
