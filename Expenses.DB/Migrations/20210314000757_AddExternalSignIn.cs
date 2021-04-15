using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.DB.Migrations
{
    public partial class AddExternalSignIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalType",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExternalType",
                table: "Users");
        }
    }
}
