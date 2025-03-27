using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StambenaZajednica.Migrations
{
    /// <inheritdoc />
    public partial class AddPinToStanar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pin",
                table: "AspNetUsers");
        }
    }
}
