using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
