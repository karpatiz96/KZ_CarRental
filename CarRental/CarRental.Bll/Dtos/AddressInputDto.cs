using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class AddressInputDto
    {
        public int Id { get; set; }
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
    }
}
