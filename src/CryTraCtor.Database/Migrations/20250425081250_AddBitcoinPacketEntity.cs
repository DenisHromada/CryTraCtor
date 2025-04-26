using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBitcoinPacketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BitcoinPacket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinPacket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitcoinPacket_FileAnalysis_FileAnalysisId",
                        column: x => x.FileAnalysisId,
                        principalTable: "FileAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinPacket_TrafficParticipant_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinPacket_TrafficParticipant_SenderId",
                        column: x => x.SenderId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacket_FileAnalysisId",
                table: "BitcoinPacket",
                column: "FileAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacket_RecipientId",
                table: "BitcoinPacket",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacket_SenderId",
                table: "BitcoinPacket",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitcoinPacket");
        }
    }
}
