using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string VehicleType { get; set; }
        public string VehicleUrl { get; set; }
        public decimal PricePerDay { get; set; }
        [Display(Name = "STAR_RATING")]
        public float StarRating { get; set; }
    }
}
