using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Dal.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "StreetAddress", "ZipCode" },
                values: new object[,]
                {
                    { 1, "Budapest", "Kacsa utca 23", 1120 },
                    { 2, "Budapest", "Fenyves utca 25", 1125 },
                    { 3, "Budapest", "Lomb utca 23", 1135 },
                    { 4, "Budapest", "Galamb utca 25", 1122 },
                    { 5, "Budapest", "Szarvas út 15", 1134 }
                });

            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "Active", "AirConditioning", "Automatic", "NumberOfDoors", "NumberOfSeats", "PricePerDay", "VehicleType", "VehicleUrl" },
                values: new object[,]
                {
                    { 18, true, true, true, 4, 5, 20000m, "Mercedes-Benz S-Class", "mb-s-klasse-4.png" },
                    { 17, true, true, true, 4, 5, 18000m, "BMW 520d", "bmw-5er-4.png" },
                    { 16, true, true, true, 5, 5, 18000m, "BMW X1", "bmw-x1-5.png" },
                    { 15, true, true, false, 4, 5, 18000m, "BMW 318d", "bmw-3er-4.png" },
                    { 14, true, true, true, 5, 5, 14000m, "Mercedes Benz B Class", "mb-b-klasse-5.png" },
                    { 13, true, true, false, 4, 4, 13000m, "Skoda Octavia", "skoda-octavia-4.png" },
                    { 12, true, true, true, 5, 5, 16000m, "Ford Mondeo", "ford-mondeo-4.png" },
                    { 11, true, true, false, 4, 5, 18000m, "Toyota Corolla Sedan", "toyota-corolla-4.png" },
                    { 10, true, true, false, 5, 5, 15000m, "Ford Focus SW", "ford-focus-kombi-5.png" },
                    { 8, true, true, false, 5, 5, 13000m, "Mini Cooper Countrman", "mini-countryman-5.png" },
                    { 19, true, true, false, 5, 7, 10000m, "Renault Grand Scenic", "renault-grand-scenic-5.png" },
                    { 7, true, true, false, 5, 5, 16000m, "Ford Focus", "ford-focus-5.png" },
                    { 6, true, true, false, 5, 5, 17000m, "Toyota Auris Hybrid", "toyota-auris-hybrid-5.png" },
                    { 5, true, true, false, 5, 5, 14000m, "Citroen C4", "citroen-c4-5.png" },
                    { 4, true, true, false, 5, 5, 13000m, "Citroen C3", "citroen-c3-5.png" },
                    { 3, true, true, false, 5, 5, 12000m, "Opel Corsa", "opel-corsa-5.png" },
                    { 2, true, true, false, 5, 4, 12000m, "Kia Picanto MCMR", "kia-picanto-5.png" },
                    { 1, true, false, false, 3, 4, 10000m, "Kia Picanto MCMN", "kia-picanto-3.png" },
                    { 9, true, true, true, 5, 5, 19000m, "BMW 116i", "bmw-5.png" },
                    { 20, true, true, false, 5, 8, 10000m, "Ford Tourneo Custom Titanium", "ford-tourneo-custom-5.png" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Active", "PlateNumber", "VehicleModelId" },
                values: new object[,]
                {
                    { 1, true, "ABC-001", 1 },
                    { 23, true, "ABC-023", 13 },
                    { 24, true, "ABC-024", 13 },
                    { 25, true, "ABC-025", 14 },
                    { 26, true, "ABC-026", 15 },
                    { 27, true, "ABC-027", 16 },
                    { 28, true, "ABC-028", 17 },
                    { 29, true, "ABC-029", 18 },
                    { 22, true, "ABC-022", 13 },
                    { 30, true, "ABC-030", 18 },
                    { 32, true, "ABC-032", 19 },
                    { 33, true, "ABC-033", 19 },
                    { 34, true, "ABC-034", 20 },
                    { 35, true, "ABC-035", 20 },
                    { 36, true, "ABC-036", 20 },
                    { 37, true, "ABC-037", 20 },
                    { 38, true, "ABC-038", 20 },
                    { 31, true, "ABC-031", 19 },
                    { 21, true, "ABC-021", 13 },
                    { 20, true, "ABC-020", 12 },
                    { 19, true, "ABC-019", 11 },
                    { 2, true, "ABC-002", 1 },
                    { 3, true, "ABC-003", 1 },
                    { 4, true, "ABC-004", 2 },
                    { 5, true, "ABC-005", 2 },
                    { 6, true, "ABC-006", 2 },
                    { 7, true, "ABC-007", 3 },
                    { 8, true, "ABC-008", 3 },
                    { 9, true, "ABC-009", 3 },
                    { 10, true, "ABC-010", 3 },
                    { 11, true, "ABC-011", 4 },
                    { 12, true, "ABC-012", 5 },
                    { 13, true, "ABC-013", 6 },
                    { 14, true, "ABC-014", 7 },
                    { 15, true, "ABC-015", 8 },
                    { 16, true, "ABC-016", 9 },
                    { 17, true, "ABC-017", 10 },
                    { 18, true, "ABC-018", 11 },
                    { 39, true, "ABC-039", 20 },
                    { 40, true, "ABC-040", 20 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
