using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStore.Infrastructure.Mobile.Data.Migrations
{
	/// <inheritdoc />
	public partial class PizzaStore : Migration
	{
		/// <inheritdoc />
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

		/// <inheritdoc />
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