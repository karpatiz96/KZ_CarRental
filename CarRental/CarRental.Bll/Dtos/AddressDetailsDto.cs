﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class AddressDetailsDto
    {
        public int Id { get; set; }
        [Display(Name = "ZIP_CODE")]
        public int ZipCode { get; set; }
        [Display(Name = "CITY")]
        public string City { get; set; }
        [Display(Name = "STREET_ADDRESS")]
        public string StreetAddress { get; set; }
        public string FullAddress { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CarDto> Cars { get; set; }
        public int CarFound { get; set; }
    }
}
