using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        public int VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; }
    }
}
