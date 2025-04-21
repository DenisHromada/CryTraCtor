using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDnsPacketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DnsPacket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false),
                    QueryName = table.Column<string>(type: "text", nullable: false),
                    QueryType = table.Column<string>(type: "text", nullable: false),
                    IsQuery = table.Column<bool>(type: "boolean", nullable: false),
                    ResponseAddresses = table.Column<string>(type: "text", nullable: true),
                    FileAnalysisId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnsPacket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DnsPacket_FileAnalysis_FileAnalysisId",
                        column: x => x.FileAnalysisId,
                        principalTable: "FileAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_FileAnalysisId",
                table: "DnsPacket",
                column: "FileAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_Timestamp",
                table: "DnsPacket",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DnsPacket");
        }
    }
}
