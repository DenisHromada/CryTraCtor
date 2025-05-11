using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class GenericPacketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenericPacket",
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
                    table.PrimaryKey("PK_GenericPacket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenericPacket_FileAnalysis_FileAnalysisId",
                        column: x => x.FileAnalysisId,
                        principalTable: "FileAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenericPacket_TrafficParticipant_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenericPacket_TrafficParticipant_SenderId",
                        column: x => x.SenderId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenericPacket_FileAnalysisId",
                table: "GenericPacket",
                column: "FileAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_GenericPacket_RecipientId",
                table: "GenericPacket",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_GenericPacket_SenderId",
                table: "GenericPacket",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenericPacket");
        }
    }
}
