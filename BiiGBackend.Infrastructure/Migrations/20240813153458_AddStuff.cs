using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiiGBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "OrderHeaders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "OrderHeaders");
        }
    }
}
