using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ThirdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Uri = table.Column<string>(type: "text", nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Location_Type = table.Column<string>(type: "text", nullable: false),
                    Location_Label_Eng = table.Column<string>(type: "text", nullable: false),
                    Location_Country_Type = table.Column<string>(type: "text", nullable: false),
                    Location_Country_Label_Eng = table.Column<string>(type: "text", nullable: false),
                    LocationValidated = table.Column<bool>(type: "boolean", nullable: false),
                    Ranking_ImportanceRank = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Uri);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Lang = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<string>(type: "text", nullable: false),
                    DateTime = table.Column<string>(type: "text", nullable: false),
                    Sim = table.Column<double>(type: "double precision", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    SourceUri = table.Column<string>(type: "text", nullable: false),
                    Links = table.Column<List<string>>(type: "text[]", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    EventUri = table.Column<string>(type: "text", nullable: false),
                    Location_Type = table.Column<string>(type: "text", nullable: false),
                    Location_Label_Eng = table.Column<string>(type: "text", nullable: false),
                    Location_Country_Type = table.Column<string>(type: "text", nullable: false),
                    Location_Country_Label_Eng = table.Column<string>(type: "text", nullable: false),
                    Shares_Facebook = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Uri);
                    table.ForeignKey(
                        name: "FK_News_Source_SourceUri",
                        column: x => x.SourceUri,
                        principalTable: "Source",
                        principalColumn: "Uri",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    IsAgency = table.Column<bool>(type: "boolean", nullable: false),
                    NewsUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Uri);
                    table.ForeignKey(
                        name: "FK_Author_News_NewsUri",
                        column: x => x.NewsUri,
                        principalTable: "News",
                        principalColumn: "Uri");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    Wgt = table.Column<int>(type: "integer", nullable: false),
                    NewsUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Uri);
                    table.ForeignKey(
                        name: "FK_Category_News_NewsUri",
                        column: x => x.NewsUri,
                        principalTable: "News",
                        principalColumn: "Uri");
                });

            migrationBuilder.CreateTable(
                name: "Concept",
                columns: table => new
                {
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    Label_Eng = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    TrendingScore_News_Score = table.Column<double>(type: "double precision", nullable: false),
                    TrendingScore_News_TestPopFq = table.Column<int>(type: "integer", nullable: false),
                    TrendingScore_News_NullPopFq = table.Column<int>(type: "integer", nullable: false),
                    Location_Type = table.Column<string>(type: "text", nullable: false),
                    Location_Label_Eng = table.Column<string>(type: "text", nullable: false),
                    Location_Country_Type = table.Column<string>(type: "text", nullable: false),
                    Location_Country_Label_Eng = table.Column<string>(type: "text", nullable: false),
                    NewsUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concept", x => x.Uri);
                    table.ForeignKey(
                        name: "FK_Concept_News_NewsUri",
                        column: x => x.NewsUri,
                        principalTable: "News",
                        principalColumn: "Uri");
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Uri = table.Column<string>(type: "text", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    NewsUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Uri);
                    table.ForeignKey(
                        name: "FK_Video_News_NewsUri",
                        column: x => x.NewsUri,
                        principalTable: "News",
                        principalColumn: "Uri");
                });

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    ViewId = table.Column<Guid>(type: "uuid", nullable: false),
                    ViewAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    NewsUri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.ViewId);
                    table.ForeignKey(
                        name: "FK_Views_News_NewsUri",
                        column: x => x.NewsUri,
                        principalTable: "News",
                        principalColumn: "Uri");
                    table.ForeignKey(
                        name: "FK_Views_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Author_NewsUri",
                table: "Author",
                column: "NewsUri");

            migrationBuilder.CreateIndex(
                name: "IX_Category_NewsUri",
                table: "Category",
                column: "NewsUri");

            migrationBuilder.CreateIndex(
                name: "IX_Concept_NewsUri",
                table: "Concept",
                column: "NewsUri");

            migrationBuilder.CreateIndex(
                name: "IX_News_SourceUri",
                table: "News",
                column: "SourceUri");

            migrationBuilder.CreateIndex(
                name: "IX_Video_NewsUri",
                table: "Video",
                column: "NewsUri");

            migrationBuilder.CreateIndex(
                name: "IX_Views_NewsUri",
                table: "Views",
                column: "NewsUri");

            migrationBuilder.CreateIndex(
                name: "IX_Views_UserId",
                table: "Views",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Concept");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Source");
        }
    }
}
