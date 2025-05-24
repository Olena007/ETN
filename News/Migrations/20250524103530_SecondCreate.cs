using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Location_LocationCountryId",
                table: "Sources");

            migrationBuilder.RenameColumn(
                name: "LocationCountryId",
                table: "Sources",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Sources_LocationCountryId",
                table: "Sources",
                newName: "IX_Sources_LocationId");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Location",
                newName: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Location_LocationId",
                table: "Sources",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Location_LocationId",
                table: "Sources");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Sources",
                newName: "LocationCountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Sources_LocationId",
                table: "Sources",
                newName: "IX_Sources_LocationCountryId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Location",
                newName: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Location_LocationCountryId",
                table: "Sources",
                column: "LocationCountryId",
                principalTable: "Location",
                principalColumn: "CountryId");
        }
    }
}
