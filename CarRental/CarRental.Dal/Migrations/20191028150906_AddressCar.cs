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
                name: "IsDeleted",
                table: "Addresses",
                nullable: false,
                defaultValue: false);

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
                name: "IsDeleted",
                table: "Addresses");
        }
    }
}
