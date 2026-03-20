using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CovAuto.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PlannerName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceTeamId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ServiceTeams_ServiceTeamId",
                        column: x => x.ServiceTeamId,
                        principalTable: "ServiceTeams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    EstimatedHours = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ScheduledFor = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_ServiceTeams_ServiceTeamId",
                        column: x => x.ServiceTeamId,
                        principalTable: "ServiceTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ServiceTeams",
                columns: new[] { "Id", "Description", "Name", "PlannerName" },
                values: new object[,]
                {
                    { 1, "Regio Noord-Holland", "Noord Service", "Linda van Noord" },
                    { 2, "Regio Zuid-Holland", "Zuid Service", "Ahmed El Farsi" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "PasswordHash", "Role", "ServiceTeamId", "Username" },
                values: new object[,]
                {
                    { 1, "Linda van Noord", "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e", "Planner", null, "planner.noord" },
                    { 2, "Ahmed El Farsi", "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e", "Planner", null, "planner.zuid" },
                    { 3, "Jan de Vries", "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e", "Monteur", 1, "monteur.jan" },
                    { 4, "Fatma Yilmaz", "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e", "Monteur", 1, "monteur.fatma" },
                    { 5, "Sven Bakker", "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e", "Monteur", 2, "monteur.sven" },
                    { 6, "Ayse Demir", "$2a$11$CtVfTc19Zo6wqxhu6FaEp.XC83OAZastaj4jl.OVrpVwcYVykck7e", "Monteur", 2, "monteur.ayse" }
                });

            migrationBuilder.InsertData(
                table: "WorkOrders",
                columns: new[] { "Id", "Address", "CreatedAt", "CustomerName", "Description", "EstimatedHours", "Priority", "ScheduledFor", "ServiceTeamId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Hoofdstraat 12, Amsterdam", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Familie Jansen", "CV-ketel geeft foutcode E5.", 3.0, "Hoog", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Nieuw", "Storing CV-ketel repareren" },
                    { 2, "Kalverstraat 88, Amsterdam", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Dhr. Peters", "Verouderde groepenkast vervangen.", 6.0, "Normaal", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Gepland", "Vervangen groepenkast" },
                    { 3, "Industrieweg 5, Haarlem", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Kantoor Bakker B.V.", "Jaarlijks onderhoud WTW-installatie.", 4.5, "Normaal", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, "InUitvoering", "Onderhoud ventilatiesysteem" },
                    { 4, "Zonnebloemstraat 7, Alkmaar", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Fam. de Groot", "Jaarlijkse inspectie en schoonmaak.", 2.0, "Laag", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Voltooid", "Inspectie zonnepanelen" },
                    { 5, "Rozenstraat 3, Zaandam", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Mevr. Smit", "Lekkage onder de keuken.", 2.5, "Hoog", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Nieuw", "Lekkage waterleidingen verhelpen" },
                    { 6, "Merenweg 21, Purmerend", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Fam. Visser", "Nieuwe warmtepomp plaatsen en aansluiten.", 8.0, "Normaal", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Gepland", "Installatie warmtepomp" },
                    { 7, "Tulpstraat 15, Amsterdam", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Dhr. Nguyen", "Geen stroom op verdieping.", 3.5, "Kritiek", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Nieuw", "Storing elektra woning" },
                    { 8, "Rozenhof 1-20, Haarlem", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "VVE Rozenhof", "Alle radiatorkranen vervangen.", 5.0, "Normaal", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Gepland", "Vervangen radiatorkranen" },
                    { 9, "Hotelplein 1, Rotterdam", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Hotel Rotterdam", "Filters reinigen en koelmiddel controleren.", 3.0, "Normaal", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Gepland", "Onderhoud airconditioning" },
                    { 10, "Bergweg 42, Rotterdam", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Fam. Hassan", "Dakgoot verstopt en lekkend.", 2.0, "Hoog", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Voltooid", "Reparatie dakgoot" },
                    { 11, "Parkweg 9, Dordrecht", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Dhr. van Dijk", "Laadpaal installeren in garage.", 4.0, "Normaal", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Nieuw", "Plaatsen laadpaal elektrisch voertuig" },
                    { 12, "Haagweg 100, Den Haag", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Woningcorp De Haag", "Storing in centrale verwarmingsinstallatie.", 6.0, "Kritiek", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, "InUitvoering", "Storing cv-ketel flatgebouw" },
                    { 13, "Winkelcentrum Zuid, Delft", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Supermarkt Koops", "Jaarlijkse veiligheidscontrole.", 1.5, "Hoog", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Voltooid", "Inspectie gasleiding" },
                    { 14, "Kelderlaan 5, Zoetermeer", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Fam. de Bruijn", "Gebarsten waterleiding in kelder.", 3.5, "Hoog", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Gepland", "Reparatie waterpijp" },
                    { 15, "Vermeerstraat 18, Leiden", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Fam. Vermeer", "Smart home systeem installeren.", 7.0, "Laag", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Nieuw", "Installatie domotica systeem" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ServiceTeamId",
                table: "Users",
                column: "ServiceTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ServiceTeamId",
                table: "WorkOrders",
                column: "ServiceTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "ServiceTeams");
        }
    }
}