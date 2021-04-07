using Microsoft.EntityFrameworkCore.Migrations;

namespace COO.DataAccess.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNC",
                table: "InfoServers");

            migrationBuilder.DropColumn(
                name: "MNC",
                table: "InfoServers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "UserName");

            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "CNC",
                table: "InfoServers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MNC",
                table: "InfoServers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
