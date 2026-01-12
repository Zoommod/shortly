using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Web.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTituloLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UrlMappings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "UrlMappings");
        }
    }
}
