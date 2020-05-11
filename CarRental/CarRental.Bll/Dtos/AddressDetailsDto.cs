using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Bll.Dtos
{
    public class AddressDetailsDto
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
        [Display(Name = "IS_IN_USE")]
        public bool IsInUse { get; set; }

        [Display(Name = "LATITUDE")]
        public float Latitude { get; set; }

        [Display(Name = "LONGITUDE")]
        public float Longitude { get; set; }

        public ICollection<CarDto> Cars { get; set; }
        public int CarFound { get; set; }

        public bool HasReservation { get; set; }
    }
}
