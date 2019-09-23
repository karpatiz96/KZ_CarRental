namespace CarRental.Bll.Filters
{
    public class VehicleModelFilter
    {
        public string VehicleType { get; set; }
        public decimal? MinPricePerDay { get; set; }
        public decimal? MaxPricePerDay { get; set; }
        public bool Active { get; set; } = true;

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } = 10;
    }
}
