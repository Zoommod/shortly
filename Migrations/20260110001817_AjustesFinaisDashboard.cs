using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shorty.Web.Migrations
{
    /// <inheritdoc />
    public partial class AjustesFinaisDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IpAdress",
                table: "UrlAcessLogs",
                newName: "IpAddress");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UrlMappings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UrlMappings");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "UrlAcessLogs",
                newName: "IpAdress");
        }
    }
}
