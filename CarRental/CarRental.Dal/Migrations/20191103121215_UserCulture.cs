using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Dal.Migrations
{
    public partial class UserCulture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Culture",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Culture",
                table: "Users");
        }
    }
}
