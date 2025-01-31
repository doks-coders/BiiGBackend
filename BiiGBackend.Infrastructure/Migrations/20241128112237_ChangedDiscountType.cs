using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiiGBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDiscountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ProductDiscountPercent",
                table: "ProductItems",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductDiscountPercent",
                table: "ProductItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
