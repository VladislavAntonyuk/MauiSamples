using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStore.Infrastructure.WebApp.Data.Migrations
{
	public partial class PizzaStore : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Description",
				table: "Pizza",
				type: "longtext",
				nullable: true)
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.AddColumn<string>(
				name: "Image",
				table: "Pizza",
				type: "longtext",
				nullable: true)
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.AddColumn<decimal>(
				name: "Price",
				table: "Pizza",
				type: "decimal(65,30)",
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