namespace CarRental.Bll.Filters
{
    public class ReservationListFilter
    {
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
            PickUpDescending,
            DropOffDescending,
            AddressDescending,
            VehicleModelDescending,
            StateDescending,
            PriceAscending,
            PriceDescending
        }
    }
}
