﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public int ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StreetAddress { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
