using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Shorty.Web.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlAcessLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataAcesso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAdress = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    UrlMappingId = table.Column<int>(type: "integer", nullable: false),
                    urlMappingsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlAcessLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlAcessLogs_UrlMappings_urlMappingsId",
                        column: x => x.urlMappingsId,
                        principalTable: "UrlMappings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlAcessLogs_urlMappingsId",
                table: "UrlAcessLogs",
                column: "urlMappingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlAcessLogs");
        }
    }
}
