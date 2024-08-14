using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiiGBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.RenameColumn(
				name: "TotalAmount",
				table: "OrderHeaders",
				newName: "TotalInNaira");

			migrationBuilder.AddColumn<double>(
                name: "LogisticsFee",
                table: "OrderHeaders",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalInDollars",
                table: "OrderHeaders",
                type: "double precision",
                nullable: true);

			migrationBuilder.AddColumn<double>(
			  name: "USDToNairaRate",
			  table: "OrderHeaders",
			  type: "double precision",
			  nullable: true);

			

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogisticsFee",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TotalInDollars",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "TotalInNaira",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "USDToNairaRate",
                table: "OrderHeaders",
                newName: "TotalAmount");
        }
    }
}
