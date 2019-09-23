using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class VehicleModelInput
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Vehicle Model")]
        public string VehicleType { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price per day")]
        [Range(0, int.MaxValue, ErrorMessage = "The price can't be negative.")]
        public decimal PricePerDay { get; set; }

        [Required]
        [Display(Name = "Picture")]
        public IFormFile Picture { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The number of doors can't be negative.")]
        [Display(Name = "Number of doors")]
        public int NumberOfDoors { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The number of seats can't be negative.")]
        [Display(Name = "Number of seats")]
        public int NumberOfSeats { get; set; }

        [Display(Name = "Automatic")]
        public bool Automatic { get; set; }

        [Display(Name = "Air Conditioned")]
        public bool AirConditioning { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
