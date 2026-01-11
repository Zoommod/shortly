using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Web.Migrations
{
    /// <inheritdoc />
    public partial class AjustarNomesFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlAcessLogs_UrlMappings_urlMappingsId",
                table: "UrlAcessLogs");

            migrationBuilder.DropIndex(
                name: "IX_UrlAcessLogs_urlMappingsId",
                table: "UrlAcessLogs");

            migrationBuilder.DropColumn(
                name: "urlMappingsId",
                table: "UrlAcessLogs");

            migrationBuilder.CreateIndex(
                name: "IX_UrlAcessLogs_UrlMappingId",
                table: "UrlAcessLogs",
                column: "UrlMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlAcessLogs_UrlMappings_UrlMappingId",
                table: "UrlAcessLogs",
                column: "UrlMappingId",
                principalTable: "UrlMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlAcessLogs_UrlMappings_UrlMappingId",
                table: "UrlAcessLogs");

            migrationBuilder.DropIndex(
                name: "IX_UrlAcessLogs_UrlMappingId",
                table: "UrlAcessLogs");

            migrationBuilder.AddColumn<int>(
                name: "urlMappingsId",
                table: "UrlAcessLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UrlAcessLogs_urlMappingsId",
                table: "UrlAcessLogs",
                column: "urlMappingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlAcessLogs_UrlMappings_urlMappingsId",
                table: "UrlAcessLogs",
                column: "urlMappingsId",
                principalTable: "UrlMappings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
