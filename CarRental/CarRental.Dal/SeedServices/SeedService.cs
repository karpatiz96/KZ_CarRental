using CarRental.Dal.Entities;
using CarRental.Dal.SeedInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRental.Dal.SeedServices
{
    public class SeedService : ISeedService
    {
        public IDictionary<string, VehicleModel> VehicleModels { get; } = new[]
        {
            new VehicleModel(){ Id = 1, VehicleType = "Kia Picanto MCMN", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/kia-picanto-3.png" ,NumberOfDoors = 3, NumberOfSeats = 4, AirConditioning = false, Automatic = false, PricePerDay = 10000, Active = true },
            new VehicleModel(){ Id = 2, VehicleType = "Kia Picanto MCMR", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/kia-picanto-5.png" ,NumberOfDoors = 5, NumberOfSeats = 4, AirConditioning = true, Automatic = false, PricePerDay = 12000, Active = true },
            new VehicleModel(){ Id = 3, VehicleType = "Opel Corsa", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/opel-corsa-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 12000, Active = true },
            new VehicleModel(){ Id = 4, VehicleType = "Citroen C3", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/citroen-c3-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 13000, Active = true },
            new VehicleModel(){ Id = 5, VehicleType = "Citroen C4", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/citroen-c4-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 14000, Active = true },
            new VehicleModel(){ Id = 6, VehicleType = "Toyota Auris Hybrid", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/toyota-auris-hybrid-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 17000, Active = true },
            new VehicleModel(){ Id = 7, VehicleType = "Ford Focus", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-focus-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 16000, Active = true },
            new VehicleModel(){ Id = 8, VehicleType = "Mini Cooper Countrman", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/mini-countryman-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 13000, Active = true },
            new VehicleModel(){ Id = 9, VehicleType = "BMW 116i", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = true, PricePerDay = 19000, Active = true },
            new VehicleModel(){ Id = 10, VehicleType = "Ford Focus SW", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-focus-kombi-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 15000, Active = true },
            new VehicleModel(){ Id = 11, VehicleType = "Toyota Corolla Sedan", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/toyota-corolla-4.png" ,NumberOfDoors = 4, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 18000, Active = true },
            new VehicleModel(){ Id = 12, VehicleType = "Ford Mondeo", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-mondeo-4.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = true, PricePerDay = 16000, Active = true },
            new VehicleModel(){ Id = 13, VehicleType = "Skoda Octavia", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/skoda-octavia-4.png" ,NumberOfDoors = 4, NumberOfSeats = 4, AirConditioning = true, Automatic = false, PricePerDay = 13000, Active = true },
            new VehicleModel(){ Id = 14, VehicleType = "Mercedes Benz B Class", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/mb-b-klasse-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = true, PricePerDay = 14000, Active = true },
            new VehicleModel(){ Id = 15, VehicleType = "BMW 318d", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-3er-4.png" ,NumberOfDoors = 4, NumberOfSeats = 5, AirConditioning = true, Automatic = false, PricePerDay = 18000, Active = true },
            new VehicleModel(){ Id = 16, VehicleType = "BMW X1", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-x1-5.png" ,NumberOfDoors = 5, NumberOfSeats = 5, AirConditioning = true, Automatic = true, PricePerDay = 18000, Active = true },
            new VehicleModel(){ Id = 17, VehicleType = "BMW 520d", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-5er-4.png" ,NumberOfDoors = 4, NumberOfSeats = 5, AirConditioning = true, Automatic = true, PricePerDay = 18000, Active = true },
            new VehicleModel(){ Id = 18, VehicleType = "Mercedes-Benz S-Class", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/mb-s-klasse-4.png" ,NumberOfDoors = 4, NumberOfSeats = 5, AirConditioning = true, Automatic = true, PricePerDay = 20000, Active = true },
            new VehicleModel(){ Id = 19, VehicleType = "Renault Grand Scenic", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/renault-grand-scenic-5.png" ,NumberOfDoors = 5, NumberOfSeats = 7, AirConditioning = true, Automatic = false, PricePerDay = 10000, Active = true },
            new VehicleModel(){ Id = 20, VehicleType = "Ford Tourneo Custom Titanium", VehicleUrl = "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-tourneo-custom-5.png" ,NumberOfDoors = 5, NumberOfSeats = 8, AirConditioning = true, Automatic = false, PricePerDay = 10000, Active = true }
        }.ToDictionary(a => a.VehicleType);

        public IList<Address> Addresses { get; }

        public IList<Car> Cars { get; }

        public SeedService()
        {
            Addresses = new List<Address>()
            {
                new Address{ Id = 1, ZipCode = 1120, City = "Budapest", StreetAddress = "Kacsa utca 23", Name = "Car Rental Kacsa utca", Latitude = 47.497913f, Longitude = 19.040236f, IsInUse = true },
                new Address{ Id = 2, ZipCode = 1125, City = "Budapest", StreetAddress = "Fenyves utca 25", Name = "Car Rental Fenyves utca", Latitude = 47.497913f, Longitude = 19.040236f, IsInUse = true },
                new Address{ Id = 3, ZipCode = 1135, City = "Budapest", StreetAddress = "Lomb utca 23", Name = "Car Rental Lomb utca", Latitude = 47.497913f, Longitude = 19.040236f, IsInUse = true },
                new Address{ Id = 4, ZipCode = 1122, City = "Budapest", StreetAddress = "Galamb utca 25", Name = "Car Rental Galamb utca", Latitude = 47.497913f, Longitude = 19.040236f, IsInUse = true },
                new Address{ Id = 5, ZipCode = 1134, City = "Budapest", StreetAddress = "Szarvas út 15", Name = "Car Rental Szarvas utca", Latitude = 47.497913f, Longitude = 19.040236f, IsInUse = true },
            };

            Cars = new List<Car>()
            {
                new Car { Id = 1, Active = true, PlateNumber = "ABC-001", VehicleModelId = VehicleModels[@"Kia Picanto MCMN"].Id },
                new Car { Id = 2, Active = true, PlateNumber = "ABC-002", VehicleModelId = VehicleModels[@"Kia Picanto MCMN"].Id },
                new Car { Id = 3, Active = true, PlateNumber = "ABC-003", VehicleModelId = VehicleModels[@"Kia Picanto MCMN"].Id },
                new Car { Id = 4, Active = true, PlateNumber = "ABC-004", VehicleModelId = VehicleModels[@"Kia Picanto MCMR"].Id },
                new Car { Id = 5, Active = true, PlateNumber = "ABC-005", VehicleModelId = VehicleModels[@"Kia Picanto MCMR"].Id },
                new Car { Id = 6, Active = true, PlateNumber = "ABC-006", VehicleModelId = VehicleModels[@"Kia Picanto MCMR"].Id },
                new Car { Id = 7, Active = true, PlateNumber = "ABC-007", VehicleModelId = VehicleModels[@"Opel Corsa"].Id },
                new Car { Id = 8, Active = true, PlateNumber = "ABC-008", VehicleModelId = VehicleModels[@"Opel Corsa"].Id },
                new Car { Id = 9, Active = true, PlateNumber = "ABC-009", VehicleModelId = VehicleModels[@"Opel Corsa"].Id },
                new Car { Id = 10, Active = true, PlateNumber = "ABC-010", VehicleModelId = VehicleModels[@"Opel Corsa"].Id },
                new Car { Id = 11, Active = true, PlateNumber = "ABC-011", VehicleModelId = VehicleModels[@"Citroen C3"].Id },
                new Car { Id = 12, Active = true, PlateNumber = "ABC-012", VehicleModelId = VehicleModels[@"Citroen C4"].Id },
                new Car { Id = 13, Active = true, PlateNumber = "ABC-013", VehicleModelId = VehicleModels[@"Toyota Auris Hybrid"].Id },
                new Car { Id = 14, Active = true, PlateNumber = "ABC-014", VehicleModelId = VehicleModels[@"Ford Focus"].Id },
                new Car { Id = 15, Active = true, PlateNumber = "ABC-015", VehicleModelId = VehicleModels[@"Mini Cooper Countrman"].Id },
                new Car { Id = 16, Active = true, PlateNumber = "ABC-016", VehicleModelId = VehicleModels[@"BMW 116i"].Id },
                new Car { Id = 17, Active = true, PlateNumber = "ABC-017", VehicleModelId = VehicleModels[@"Ford Focus SW"].Id },
                new Car { Id = 18, Active = true, PlateNumber = "ABC-018", VehicleModelId = VehicleModels[@"Toyota Corolla Sedan"].Id },
                new Car { Id = 19, Active = true, PlateNumber = "ABC-019", VehicleModelId = VehicleModels[@"Toyota Corolla Sedan"].Id },
                new Car { Id = 20, Active = true, PlateNumber = "ABC-020", VehicleModelId = VehicleModels[@"Ford Mondeo"].Id },

                new Car { Id = 21, Active = true, PlateNumber = "ABC-021", VehicleModelId = VehicleModels[@"Skoda Octavia"].Id },
                new Car { Id = 22, Active = true, PlateNumber = "ABC-022", VehicleModelId = VehicleModels[@"Skoda Octavia"].Id },
                new Car { Id = 23, Active = true, PlateNumber = "ABC-023", VehicleModelId = VehicleModels[@"Skoda Octavia"].Id },
                new Car { Id = 24, Active = true, PlateNumber = "ABC-024", VehicleModelId = VehicleModels[@"Skoda Octavia"].Id },
                new Car { Id = 25, Active = true, PlateNumber = "ABC-025", VehicleModelId = VehicleModels[@"Mercedes Benz B Class"].Id },
                new Car { Id = 26, Active = true, PlateNumber = "ABC-026", VehicleModelId = VehicleModels[@"BMW 318d"].Id },
                new Car { Id = 27, Active = true, PlateNumber = "ABC-027", VehicleModelId = VehicleModels[@"BMW X1"].Id },
                new Car { Id = 28, Active = true, PlateNumber = "ABC-028", VehicleModelId = VehicleModels[@"BMW 520d"].Id },
                new Car { Id = 29, Active = true, PlateNumber = "ABC-029", VehicleModelId = VehicleModels[@"Mercedes-Benz S-Class"].Id },
                new Car { Id = 30, Active = true, PlateNumber = "ABC-030", VehicleModelId = VehicleModels[@"Mercedes-Benz S-Class"].Id },
                new Car { Id = 31, Active = true, PlateNumber = "ABC-031", VehicleModelId = VehicleModels[@"Renault Grand Scenic"].Id },
                new Car { Id = 32, Active = true, PlateNumber = "ABC-032", VehicleModelId = VehicleModels[@"Renault Grand Scenic"].Id },
                new Car { Id = 33, Active = true, PlateNumber = "ABC-033", VehicleModelId = VehicleModels[@"Renault Grand Scenic"].Id },
                new Car { Id = 34, Active = true, PlateNumber = "ABC-034", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
                new Car { Id = 35, Active = true, PlateNumber = "ABC-035", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
                new Car { Id = 36, Active = true, PlateNumber = "ABC-036", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
                new Car { Id = 37, Active = true, PlateNumber = "ABC-037", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
                new Car { Id = 38, Active = true, PlateNumber = "ABC-038", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
                new Car { Id = 39, Active = true, PlateNumber = "ABC-039", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
                new Car { Id = 40, Active = true, PlateNumber = "ABC-040", VehicleModelId = VehicleModels[@"Ford Tourneo Custom Titanium"].Id },
            };
        }
    }
}
