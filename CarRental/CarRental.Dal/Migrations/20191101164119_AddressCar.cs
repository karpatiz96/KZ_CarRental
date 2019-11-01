using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Dal.Migrations
{
    public partial class AddressCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInUse",
                table: "Addresses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsInUse",
                value: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsInUse",
                value: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsInUse",
                value: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsInUse",
                value: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsInUse",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_AddressId",
                table: "Cars",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Addresses_AddressId",
                table: "Cars",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Addresses_AddressId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_AddressId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "IsInUse",
                table: "Addresses");
        }
    }
}
