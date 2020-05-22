using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web
{
    public class CarReservationDetails
    {
        public string VehicleModelName { get; set; }

        public string PickUpTime { get; set; }

        public DateTime PickUpTimeValue { get; set; }

        public string DropOffTime { get; set; }

        public DateTime DropOffTimeValue { get; set; }
    }
}
