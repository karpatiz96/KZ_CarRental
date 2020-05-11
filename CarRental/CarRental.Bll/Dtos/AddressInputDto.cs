using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class AddressInputDto
    {
        public int Id { get; set; }
        [Display(Name = "SITE_NAME")]
        public string Name { get; set; }
        [Required(ErrorMessage = "ZIP_CODE_REQUIRED")]
        [Display(Name = "ZIP_CODE")]
        [Range(1000, 9999, ErrorMessage = "ZIP_CODE_RANGE")]
        public int? ZipCode { get; set; }
        [Required(ErrorMessage = "CITY_REQUIRED")]
        [Display(Name = "CITY")]
        public string City { get; set; }
        [Required(ErrorMessage = "STREET_ADDRESS_REQUIRED")]
        [Display(Name = "STREET_ADDRESS")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "LATITUDE_REQUIRED")]
        [Range(-90.0, 90.0, ErrorMessage = "LATITUDE_RANGE")]
        [Display(Name = "LATITUDE")]
        public float Latitude { get; set; }

        [Required(ErrorMessage = "LONGITUDE_REQUIRED")]
        [Range(-180.0, 180.0, ErrorMessage = "LONGITUDE_RANGE")]
        [Display(Name = "LONGITUDE")]
        public float Longitude { get; set; }

        [Display(Name = "IS_IN_USE")]
        public bool IsInUse { get; set; }
    }
}
