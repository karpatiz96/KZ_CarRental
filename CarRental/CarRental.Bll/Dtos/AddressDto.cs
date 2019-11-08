using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        [Display(Name = "SITE_NAME")]
        public string Name { get; set; }
        [Display(Name = "ZIP_CODE")]
        public int ZipCode { get; set; }
        [Display(Name = "CITY")]
        public string City { get; set; }
        [Display(Name = "STREET_ADDRESS")]
        public string StreetAddress { get; set; }
        public string FullAddress { get; set; }
    }
}
