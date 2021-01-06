using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace COO.Server.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Server = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Affiliation = table.Column<string>(type: "text", nullable: true),
                    XP = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Inv = table.Column<string>(type: "text", nullable: true),
                    Equips = table.Column<string>(type: "text", nullable: true),
                    Skills = table.Column<string>(type: "text", nullable: true),
                    Talents = table.Column<string>(type: "text", nullable: true),
                    Appearance = table.Column<string>(type: "text", nullable: true),
                    Gameplay = table.Column<string>(type: "text", nullable: true),
                    Keybinds = table.Column<string>(type: "text", nullable: true),
                    KeyRemap = table.Column<string>(type: "text", nullable: true),
                    Chat = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DS_CreationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Region = table.Column<string>(type: "text", nullable: true),
                    ServerType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_CreationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DS_HostInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hosts = table.Column<string>(type: "text", nullable: true),
                    ServerType = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    MNP = table.Column<int>(type: "integer", nullable: false),
                    PG = table.Column<string>(type: "text", nullable: true),
                    IG = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_HostInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DS_LoginRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_LoginRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LFGs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Requester = table.Column<string>(type: "text", nullable: true),
                    Members = table.Column<string>(type: "text", nullable: true),
                    HostRequest = table.Column<string>(type: "text", nullable: true),
                    GameType = table.Column<string>(type: "text", nullable: true),
                    TeamCount = table.Column<int>(type: "integer", nullable: false),
                    MNP = table.Column<int>(type: "integer", nullable: false),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LFGs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    IsLogin = table.Column<bool>(type: "boolean", nullable: false),
                    MainIP = table.Column<string>(type: "text", nullable: true),
                    InstanceIP = table.Column<string>(type: "text", nullable: true),
                    PotentialGI = table.Column<string>(type: "text", nullable: true),
                    Alert = table.Column<string>(type: "text", nullable: true),
                    CurrentChar = table.Column<string>(type: "text", nullable: true),
                    CurrentParty = table.Column<string>(type: "text", nullable: true),
                    Leader = table.Column<string>(type: "text", nullable: true),
                    XServerMessages = table.Column<string>(type: "text", nullable: true),
                    GIReady = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaveUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    Verification = table.Column<string>(type: "text", nullable: true),
                    BanTime = table.Column<int>(type: "integer", nullable: false),
                    PrevIP = table.Column<string>(type: "text", nullable: true),
                    PrevLogin = table.Column<string>(type: "text", nullable: true),
                    PrevDevice = table.Column<string>(type: "text", nullable: true),
                    FavServers = table.Column<string>(type: "text", nullable: true),
                    CharLimit = table.Column<int>(type: "integer", nullable: false),
                    FriendList = table.Column<string>(type: "text", nullable: true),
                    BlockedList = table.Column<string>(type: "text", nullable: true),
                    Privacy = table.Column<string>(type: "text", nullable: true),
                    BankInv = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServerType = table.Column<string>(type: "text", nullable: true),
                    IP = table.Column<string>(type: "text", nullable: true),
                    Port = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    IsInGame = table.Column<bool>(type: "boolean", nullable: false),
                    CNP = table.Column<int>(type: "integer", nullable: false),
                    MNP = table.Column<int>(type: "integer", nullable: false),
                    PG = table.Column<string>(type: "text", nullable: true),
                    IG = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharUsers");

            migrationBuilder.DropTable(
                name: "DS_CreationRequests");

            migrationBuilder.DropTable(
                name: "DS_HostInfos");

            migrationBuilder.DropTable(
                name: "DS_LoginRequests");

            migrationBuilder.DropTable(
                name: "LFGs");

            migrationBuilder.DropTable(
                name: "PlayUsers");

            migrationBuilder.DropTable(
                name: "SaveUsers");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
