using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeminiEmbeddingDimensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Vector>(
                name: "Vector",
                table: "ArticleEmbeddingsGemini",
                type: "vector(3072)",
                nullable: false,
                oldClrType: typeof(Vector),
                oldType: "vector(768)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Vector>(
                name: "Vector",
                table: "ArticleEmbeddingsGemini",
                type: "vector(768)",
                nullable: false,
                oldClrType: typeof(Vector),
                oldType: "vector(3072)");
        }
    }
}
