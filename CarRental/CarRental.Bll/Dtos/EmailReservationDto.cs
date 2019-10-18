using System;
using System.Collections.Generic;
using System.Text;
using static CarRental.Dal.Entities.Reservation;

namespace CarRental.Bll.Dtos
{
    public class EmailReservationDto
    {
        public string UserName { get; set; }

        public string VehicleType { get; set; }

        public DateTime PickUpTime { get; set; }

        public DateTime DropOffTime { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public ReservationStates State { get; set; }
    }
}
