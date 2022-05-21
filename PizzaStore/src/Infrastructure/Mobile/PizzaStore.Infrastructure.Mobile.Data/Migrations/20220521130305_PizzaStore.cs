using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStore.Infrastructure.Mobile.Data.Migrations
{
    public partial class PizzaStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pizza",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Pizza",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Pizza",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pizza");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Pizza");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Pizza");
        }
    }
}
