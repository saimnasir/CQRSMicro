using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CQRSMicro.Sale.Migrations
{
    public partial class SaleProductQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SaleProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SaleProduct");
        }
    }
}
