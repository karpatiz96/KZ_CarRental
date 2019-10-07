using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        public int VehicleModelId { get; set; }
        public  VehicleModel VehicleModel { get; set; }
    }
}
