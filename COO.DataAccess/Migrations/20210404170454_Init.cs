using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace COO.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    ClanId = table.Column<int>(type: "integer", nullable: true),
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: false),
                    Mana = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    PosX = table.Column<double>(type: "double precision", nullable: false),
                    PosY = table.Column<double>(type: "double precision", nullable: false),
                    PosZ = table.Column<double>(type: "double precision", nullable: false),
                    RotationYaw = table.Column<double>(type: "double precision", nullable: false),
                    EquipChest = table.Column<string>(type: "text", nullable: true),
                    EquipFeet = table.Column<string>(type: "text", nullable: true),
                    EquipHands = table.Column<string>(type: "text", nullable: true),
                    EquipHead = table.Column<string>(type: "text", nullable: true),
                    EquipLegs = table.Column<string>(type: "text", nullable: true),
                    Hotbar0 = table.Column<string>(type: "text", nullable: true),
                    Hotbar1 = table.Column<string>(type: "text", nullable: true),
                    Hotbar2 = table.Column<string>(type: "text", nullable: true),
                    Hotbar3 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaderId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InfoServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IP = table.Column<string>(type: "text", nullable: true),
                    Port = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MapName = table.Column<string>(type: "text", nullable: true),
                    interiorIP = table.Column<string>(type: "text", nullable: true),
                    interiorPort = table.Column<string>(type: "text", nullable: true),
                    ExternalIP = table.Column<string>(type: "text", nullable: true),
                    ExternalPort = table.Column<string>(type: "text", nullable: true),
                    CurrentCount = table.Column<int>(type: "integer", nullable: false),
                    MaxCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CNC = table.Column<int>(type: "integer", nullable: false),
                    MNC = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InitializeDataCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: false),
                    Mana = table.Column<int>(type: "integer", nullable: false),
                    PosX = table.Column<double>(type: "double precision", nullable: false),
                    PosY = table.Column<double>(type: "double precision", nullable: false),
                    PosZ = table.Column<double>(type: "double precision", nullable: false),
                    RotationYaw = table.Column<double>(type: "double precision", nullable: false),
                    EquipChest = table.Column<string>(type: "text", nullable: true),
                    EquipFeet = table.Column<string>(type: "text", nullable: true),
                    EquipHands = table.Column<string>(type: "text", nullable: true),
                    EquipHead = table.Column<string>(type: "text", nullable: true),
                    EquipLegs = table.Column<string>(type: "text", nullable: true),
                    Hotbar0 = table.Column<string>(type: "text", nullable: true),
                    Hotbar1 = table.Column<string>(type: "text", nullable: true),
                    Hotbar2 = table.Column<string>(type: "text", nullable: true),
                    Hotbar3 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitializeDataCharacters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Item = table.Column<string>(type: "text", nullable: true),
                    Slot = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    Task1 = table.Column<int>(type: "integer", nullable: false),
                    Task2 = table.Column<int>(type: "integer", nullable: false),
                    Task3 = table.Column<int>(type: "integer", nullable: false),
                    Task4 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PaswordSalt = table.Column<string>(type: "text", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    IP = table.Column<string>(type: "text", nullable: true),
                    CountFailedLogins = table.Column<int>(type: "integer", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Clans");

            migrationBuilder.DropTable(
                name: "InfoServers");

            migrationBuilder.DropTable(
                name: "InitializeDataCharacters");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
