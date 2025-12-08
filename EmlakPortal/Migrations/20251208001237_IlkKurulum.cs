using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmlakPortal.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IlanDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IlanDurumlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IlanTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IlanTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Evler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ResimUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IlanTurId = table.Column<int>(type: "int", nullable: false),
                    IlanDurumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evler_IlanDurumlari_IlanDurumId",
                        column: x => x.IlanDurumId,
                        principalTable: "IlanDurumlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evler_IlanTurleri_IlanTurId",
                        column: x => x.IlanTurId,
                        principalTable: "IlanTurleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evler_IlanDurumId",
                table: "Evler",
                column: "IlanDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_Evler_IlanTurId",
                table: "Evler",
                column: "IlanTurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evler");

            migrationBuilder.DropTable(
                name: "IlanDurumlari");

            migrationBuilder.DropTable(
                name: "IlanTurleri");
        }
    }
}
