namespace CarRental.Bll.Filters
{
    public class ReservationFilter
    {
        public string VehicleType { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; } = 10;

        public ReservationOrder reservationOrder { get; set; }

        public enum ReservationOrder
        {
            PickUpAscending,
            DropOffAscending,
            AddressAscending,
            VehicleModelAscending,
            StateAscending,
            CarAscending,
            PickUpDescending,
            DropOffDescending,
            AddressDescending,
            VehicleModelDescending,
            StateDescending,
            CarDescending,
        }
    }
}
