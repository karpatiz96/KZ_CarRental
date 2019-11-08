using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Bll.Filters
{
    public class AddressFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } = 10;

        public bool Active { get; set; }

        public AddressOrder addressOrder { get; set; }

        public enum AddressOrder
        {
            ZipCodeAscending,
            CityAscending,
            StreetAddressAscending,
            ZipCodeDescending,
            CityDescending,
            StreetAddressDescending,
            NameAscending,
            NameDescending
        }
    }
}
