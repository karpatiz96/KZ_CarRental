using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Dal.Migrations
{
    public partial class Seed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/kia-picanto-3.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/kia-picanto-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/opel-corsa-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/citroen-c3-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 5,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/citroen-c4-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 6,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/toyota-auris-hybrid-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 7,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-focus-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 8,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/mini-countryman-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 9,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 10,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-focus-kombi-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 11,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/toyota-corolla-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 12,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-mondeo-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 13,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/skoda-octavia-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 14,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/mb-b-klasse-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 15,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-3er-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 16,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-x1-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 17,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/bmw-5er-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 18,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/mb-s-klasse-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 19,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/renault-grand-scenic-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 20,
                column: "VehicleUrl",
                value: "https://carrentalwebkzstorage.blob.core.windows.net/images/ford-tourneo-custom-5.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "VehicleUrl",
                value: "kia-picanto-3.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "VehicleUrl",
                value: "kia-picanto-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "VehicleUrl",
                value: "opel-corsa-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "VehicleUrl",
                value: "citroen-c3-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 5,
                column: "VehicleUrl",
                value: "citroen-c4-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 6,
                column: "VehicleUrl",
                value: "toyota-auris-hybrid-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 7,
                column: "VehicleUrl",
                value: "ford-focus-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 8,
                column: "VehicleUrl",
                value: "mini-countryman-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 9,
                column: "VehicleUrl",
                value: "bmw-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 10,
                column: "VehicleUrl",
                value: "ford-focus-kombi-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 11,
                column: "VehicleUrl",
                value: "toyota-corolla-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 12,
                column: "VehicleUrl",
                value: "ford-mondeo-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 13,
                column: "VehicleUrl",
                value: "skoda-octavia-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 14,
                column: "VehicleUrl",
                value: "mb-b-klasse-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 15,
                column: "VehicleUrl",
                value: "bmw-3er-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 16,
                column: "VehicleUrl",
                value: "bmw-x1-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 17,
                column: "VehicleUrl",
                value: "bmw-5er-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 18,
                column: "VehicleUrl",
                value: "mb-s-klasse-4.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 19,
                column: "VehicleUrl",
                value: "renault-grand-scenic-5.png");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 20,
                column: "VehicleUrl",
                value: "ford-tourneo-custom-5.png");
        }
    }
}
