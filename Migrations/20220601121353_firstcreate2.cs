using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5thBridgeShop.Migrations
{
    public partial class firstcreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Carts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Carts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
