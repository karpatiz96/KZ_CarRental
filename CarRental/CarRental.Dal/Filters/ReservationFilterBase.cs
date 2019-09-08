﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Dal.Filters
{
    public class ReservationFilterBase
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } = 10;

        public ReservationOrder reservationOrder { get; set; }

        public enum ReservationOrder
        {
            PickUpAscending,
            DropOffAscending,
            AddressAscending,
            PriceAscending,
            VehicleModelAscending,
            StateAscending,
            PickUpDescending,
            DropOffDescending,
            AddressDescending,
            PriceDescending,
            VehicleModelDescending,
            StateDescending
        }
    }
}
