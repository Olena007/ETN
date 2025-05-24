using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Lang = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<string>(type: "text", nullable: false),
                    DateTime = table.Column<string>(type: "text", nullable: false),
                    Sim = table.Column<double>(type: "double precision", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Links = table.Column<List<string>>(type: "text[]", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    EventUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    UserSurname = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<string>(type: "text", nullable: true),
                    UserEmail = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    IsAgency = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                    table.ForeignKey(
                        name: "FK_Authors_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: true),
                    Wgt = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.CountryId);
                    table.ForeignKey(
                        name: "FK_Location_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    VideoId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.VideoId);
                    table.ForeignKey(
                        name: "FK_Videos_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    ViewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ViewAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.ViewId);
                    table.ForeignKey(
                        name: "FK_Views_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Views_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                    table.ForeignKey(
                        name: "FK_Country_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LocationCountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocationValidated = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.SourceId);
                    table.ForeignKey(
                        name: "FK_Sources_Location_LocationCountryId",
                        column: x => x.LocationCountryId,
                        principalTable: "Location",
                        principalColumn: "CountryId");
                    table.ForeignKey(
                        name: "FK_Sources_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "NewsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_NewsId",
                table: "Authors",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_NewsId",
                table: "Categories",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_LocationId",
                table: "Country",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_NewsId",
                table: "Location",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_LocationCountryId",
                table: "Sources",
                column: "LocationCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_NewsId",
                table: "Sources",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_NewsId",
                table: "Videos",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Views_NewsId",
                table: "Views",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Views_UserId",
                table: "Views",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
