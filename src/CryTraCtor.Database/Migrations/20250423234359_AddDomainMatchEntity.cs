using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainMatchEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomainMatch",
                columns: table => new
                {
                    KnownDomainId = table.Column<Guid>(type: "uuid", nullable: false),
                    DnsPacketId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainMatch", x => new { x.KnownDomainId, x.DnsPacketId });
                    table.ForeignKey(
                        name: "FK_DomainMatch_DnsPacket_DnsPacketId",
                        column: x => x.DnsPacketId,
                        principalTable: "DnsPacket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainMatch_KnownDomain_KnownDomainId",
                        column: x => x.KnownDomainId,
                        principalTable: "KnownDomain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainMatch_DnsPacketId",
                table: "DomainMatch",
                column: "DnsPacketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainMatch");
        }
    }
}
