namespace CarRental.Bll.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string VehicleType { get; set; }
        public string VehicleUrl { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
