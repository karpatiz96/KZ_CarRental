using System.Collections.Generic;

namespace CarRental.Bll.Dtos
{
    public class PagedResult<T>
    {
        public IList<T> Results { get; set; }
        public int? Total { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
