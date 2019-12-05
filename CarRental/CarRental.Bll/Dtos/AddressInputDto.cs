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
        [Display(Name = "IS_IN_USE")]
        public bool IsInUse { get; set; }
    }
}
