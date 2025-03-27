using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StambenaZajednica.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationRepositoryDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StanarId",
                table: "Stanovi",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StanarId1",
                table: "Stanovi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stanar",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stanovi_StanarId1",
                table: "Stanovi",
                column: "StanarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Stanovi_Stanar_StanarId1",
                table: "Stanovi",
                column: "StanarId1",
                principalTable: "Stanar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stanovi_Stanar_StanarId1",
                table: "Stanovi");

            migrationBuilder.DropTable(
                name: "Stanar");

            migrationBuilder.DropIndex(
                name: "IX_Stanovi_StanarId1",
                table: "Stanovi");

            migrationBuilder.DropColumn(
                name: "StanarId",
                table: "Stanovi");

            migrationBuilder.DropColumn(
                name: "StanarId1",
                table: "Stanovi");
        }
    }
}
