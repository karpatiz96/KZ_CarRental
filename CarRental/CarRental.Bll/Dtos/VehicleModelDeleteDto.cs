namespace CarRental.Bll.Dtos
{
    public class VehicleModelDeleteDto : VehicleModelDto
    {
        public bool HasReservation { get; set; }
    }
}
