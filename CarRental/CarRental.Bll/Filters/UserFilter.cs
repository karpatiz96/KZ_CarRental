using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Bll.Filters
{
    public class UserFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } = 10;

        public string RoleName { get; set; }
        public UserOrder userOrder { get; set; }

        public enum UserOrder
        {
            IdAscending = 0,
            IdDescending = 1,
            NameAscending = 2,
            NameDescending = 3,
            EmailAscending = 4,
            EmailDescending = 5
        }
    }
}
