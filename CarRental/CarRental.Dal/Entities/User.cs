using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class User : IdentityUser<int>
    {
        [PersonalData]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Culture { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
