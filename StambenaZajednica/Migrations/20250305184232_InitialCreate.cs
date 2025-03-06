using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StambenaZajednica.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StambeneZajednice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojZgrade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StambeneZajednice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Finansije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipTroska = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IznosDuga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DatumDospeca = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IznosUplate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DatumUplate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StambenaZajednicaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finansije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finansije_StambeneZajednice_StambenaZajednicaId",
                        column: x => x.StambenaZajednicaId,
                        principalTable: "StambeneZajednice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stanovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojStana = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaLiJeZakljucan = table.Column<bool>(type: "bit", nullable: false),
                    StambenaZajednicaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stanovi_StambeneZajednice_StambenaZajednicaId",
                        column: x => x.StambenaZajednicaId,
                        principalTable: "StambeneZajednice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Finansije_StambenaZajednicaId",
                table: "Finansije",
                column: "StambenaZajednicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Stanovi_StambenaZajednicaId",
                table: "Stanovi",
                column: "StambenaZajednicaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Finansije");

            migrationBuilder.DropTable(
                name: "Stanovi");

            migrationBuilder.DropTable(
                name: "StambeneZajednice");
        }
    }
}
