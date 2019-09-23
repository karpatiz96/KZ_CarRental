namespace CarRental.Bll.Filters
{
    public class CarFilter
    {
        public string VehicleType { get; set; }
        public string PlateNumber { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } = 10;

        public CarOrder carOrder { get; set; }

        public enum CarOrder
        {
            PlateNumberAscending,
            VehicleTypeAscending,
            ActiveAscending,
            PlateNumberDescending,
            VehicleTypeDescending,
            ActiveDescending
        }
    }
}
