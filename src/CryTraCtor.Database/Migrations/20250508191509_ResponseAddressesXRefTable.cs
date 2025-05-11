using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class ResponseAddressesXRefTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseAddresses",
                table: "DnsMessage");

            migrationBuilder.CreateTable(
                name: "DnsMessageResolvedTrafficParticipant",
                columns: table => new
                {
                    DnsMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrafficParticipantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnsMessageResolvedTrafficParticipant", x => new { x.DnsMessageId, x.TrafficParticipantId });
                    table.ForeignKey(
                        name: "FK_DnsMessageResolvedTrafficParticipant_DnsMessage_DnsMessageId",
                        column: x => x.DnsMessageId,
                        principalTable: "DnsMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnsMessageResolvedTrafficParticipant_TrafficParticipant_Tra~",
                        column: x => x.TrafficParticipantId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DnsMessageResolvedTrafficParticipant_TrafficParticipantId",
                table: "DnsMessageResolvedTrafficParticipant",
                column: "TrafficParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DnsMessageResolvedTrafficParticipant");

            migrationBuilder.AddColumn<string>(
                name: "ResponseAddresses",
                table: "DnsMessage",
                type: "text",
                nullable: true);
        }
    }
}
