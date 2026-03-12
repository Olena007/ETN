using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPgVector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.AlterColumn<Vector>(
                name: "Vector",
                table: "ArticleEmbeddings",
                type: "vector(384)",
                nullable: false,
                oldClrType: typeof(float[]),
                oldType: "real[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.AlterColumn<float[]>(
                name: "Vector",
                table: "ArticleEmbeddings",
                type: "real[]",
                nullable: false,
                oldClrType: typeof(Vector),
                oldType: "vector(384)");
        }
    }
}
