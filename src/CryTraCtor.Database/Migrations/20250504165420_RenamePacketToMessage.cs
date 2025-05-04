using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenamePacketToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainMatch_DnsPacket_DnsPacketId",
                table: "DomainMatch");

            migrationBuilder.DropTable(
                name: "BitcoinPacketHeader");

            migrationBuilder.DropTable(
                name: "BitcoinPacketInventory");

            migrationBuilder.DropTable(
                name: "BitcoinPacketTransaction");

            migrationBuilder.DropTable(
                name: "DnsPacket");

            migrationBuilder.DropTable(
                name: "BitcoinPacket");

            migrationBuilder.RenameColumn(
                name: "DnsPacketId",
                table: "DomainMatch",
                newName: "DnsMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_DomainMatch_DnsPacketId",
                table: "DomainMatch",
                newName: "IX_DomainMatch_DnsMessageId");

            migrationBuilder.CreateTable(
                name: "BitcoinMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Magic = table.Column<long>(type: "bigint", nullable: false),
                    Command = table.Column<string>(type: "text", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Checksum = table.Column<long>(type: "bigint", nullable: false),
                    FileAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitcoinMessage_FileAnalysis_FileAnalysisId",
                        column: x => x.FileAnalysisId,
                        principalTable: "FileAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinMessage_TrafficParticipant_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinMessage_TrafficParticipant_SenderId",
                        column: x => x.SenderId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DnsMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false),
                    QueryName = table.Column<string>(type: "text", nullable: false),
                    QueryType = table.Column<string>(type: "text", nullable: false),
                    IsQuery = table.Column<bool>(type: "boolean", nullable: false),
                    ResponseAddresses = table.Column<string>(type: "text", nullable: true),
                    FileAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnsMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DnsMessage_FileAnalysis_FileAnalysisId",
                        column: x => x.FileAnalysisId,
                        principalTable: "FileAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnsMessage_TrafficParticipant_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnsMessage_TrafficParticipant_SenderId",
                        column: x => x.SenderId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinMessageHeader",
                columns: table => new
                {
                    BitcoinMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinBlockHeaderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinMessageHeader", x => new { x.BitcoinMessageId, x.BitcoinBlockHeaderId });
                    table.ForeignKey(
                        name: "FK_BitcoinMessageHeader_BitcoinBlockHeader_BitcoinBlockHeaderId",
                        column: x => x.BitcoinBlockHeaderId,
                        principalTable: "BitcoinBlockHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinMessageHeader_BitcoinMessage_BitcoinMessageId",
                        column: x => x.BitcoinMessageId,
                        principalTable: "BitcoinMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinMessageInventory",
                columns: table => new
                {
                    BitcoinMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinInventoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinMessageInventory", x => new { x.BitcoinMessageId, x.BitcoinInventoryId });
                    table.ForeignKey(
                        name: "FK_BitcoinMessageInventory_BitcoinInventory_BitcoinInventoryId",
                        column: x => x.BitcoinInventoryId,
                        principalTable: "BitcoinInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinMessageInventory_BitcoinMessage_BitcoinMessageId",
                        column: x => x.BitcoinMessageId,
                        principalTable: "BitcoinMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinMessageTransaction",
                columns: table => new
                {
                    BitcoinMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinTransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinMessageTransaction", x => new { x.BitcoinMessageId, x.BitcoinTransactionId });
                    table.ForeignKey(
                        name: "FK_BitcoinMessageTransaction_BitcoinMessage_BitcoinMessageId",
                        column: x => x.BitcoinMessageId,
                        principalTable: "BitcoinMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinMessageTransaction_BitcoinTransaction_BitcoinTransac~",
                        column: x => x.BitcoinTransactionId,
                        principalTable: "BitcoinTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinMessage_FileAnalysisId",
                table: "BitcoinMessage",
                column: "FileAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinMessage_RecipientId",
                table: "BitcoinMessage",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinMessage_SenderId",
                table: "BitcoinMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinMessageHeader_BitcoinBlockHeaderId",
                table: "BitcoinMessageHeader",
                column: "BitcoinBlockHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinMessageInventory_BitcoinInventoryId",
                table: "BitcoinMessageInventory",
                column: "BitcoinInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinMessageTransaction_BitcoinTransactionId",
                table: "BitcoinMessageTransaction",
                column: "BitcoinTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsMessage_FileAnalysisId",
                table: "DnsMessage",
                column: "FileAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsMessage_RecipientId",
                table: "DnsMessage",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsMessage_SenderId",
                table: "DnsMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsMessage_Timestamp",
                table: "DnsMessage",
                column: "Timestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainMatch_DnsMessage_DnsMessageId",
                table: "DomainMatch",
                column: "DnsMessageId",
                principalTable: "DnsMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DomainMatch_DnsMessage_DnsMessageId",
                table: "DomainMatch");

            migrationBuilder.DropTable(
                name: "BitcoinMessageHeader");

            migrationBuilder.DropTable(
                name: "BitcoinMessageInventory");

            migrationBuilder.DropTable(
                name: "BitcoinMessageTransaction");

            migrationBuilder.DropTable(
                name: "DnsMessage");

            migrationBuilder.DropTable(
                name: "BitcoinMessage");

            migrationBuilder.RenameColumn(
                name: "DnsMessageId",
                table: "DomainMatch",
                newName: "DnsPacketId");

            migrationBuilder.RenameIndex(
                name: "IX_DomainMatch_DnsMessageId",
                table: "DomainMatch",
                newName: "IX_DomainMatch_DnsPacketId");

            migrationBuilder.CreateTable(
                name: "BitcoinPacket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Checksum = table.Column<long>(type: "bigint", nullable: false),
                    Command = table.Column<string>(type: "text", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Magic = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DnsPacket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsQuery = table.Column<bool>(type: "boolean", nullable: false),
                    QueryName = table.Column<string>(type: "text", nullable: false),
                    QueryType = table.Column<string>(type: "text", nullable: false),
                    ResponseAddresses = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_DnsPacket_TrafficParticipant_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnsPacket_TrafficParticipant_SenderId",
                        column: x => x.SenderId,
                        principalTable: "TrafficParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinPacketHeader",
                columns: table => new
                {
                    BitcoinPacketId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinBlockHeaderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinPacketHeader", x => new { x.BitcoinPacketId, x.BitcoinBlockHeaderId });
                    table.ForeignKey(
                        name: "FK_BitcoinPacketHeader_BitcoinBlockHeader_BitcoinBlockHeaderId",
                        column: x => x.BitcoinBlockHeaderId,
                        principalTable: "BitcoinBlockHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinPacketHeader_BitcoinPacket_BitcoinPacketId",
                        column: x => x.BitcoinPacketId,
                        principalTable: "BitcoinPacket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinPacketInventory",
                columns: table => new
                {
                    BitcoinPacketId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinInventoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinPacketInventory", x => new { x.BitcoinPacketId, x.BitcoinInventoryId });
                    table.ForeignKey(
                        name: "FK_BitcoinPacketInventory_BitcoinInventory_BitcoinInventoryId",
                        column: x => x.BitcoinInventoryId,
                        principalTable: "BitcoinInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinPacketInventory_BitcoinPacket_BitcoinPacketId",
                        column: x => x.BitcoinPacketId,
                        principalTable: "BitcoinPacket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinPacketTransaction",
                columns: table => new
                {
                    BitcoinPacketId = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinTransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinPacketTransaction", x => new { x.BitcoinPacketId, x.BitcoinTransactionId });
                    table.ForeignKey(
                        name: "FK_BitcoinPacketTransaction_BitcoinPacket_BitcoinPacketId",
                        column: x => x.BitcoinPacketId,
                        principalTable: "BitcoinPacket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BitcoinPacketTransaction_BitcoinTransaction_BitcoinTransact~",
                        column: x => x.BitcoinTransactionId,
                        principalTable: "BitcoinTransaction",
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

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacketHeader_BitcoinBlockHeaderId",
                table: "BitcoinPacketHeader",
                column: "BitcoinBlockHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacketInventory_BitcoinInventoryId",
                table: "BitcoinPacketInventory",
                column: "BitcoinInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacketTransaction_BitcoinTransactionId",
                table: "BitcoinPacketTransaction",
                column: "BitcoinTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_FileAnalysisId",
                table: "DnsPacket",
                column: "FileAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_RecipientId",
                table: "DnsPacket",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_SenderId",
                table: "DnsPacket",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_Timestamp",
                table: "DnsPacket",
                column: "Timestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_DomainMatch_DnsPacket_DnsPacketId",
                table: "DomainMatch",
                column: "DnsPacketId",
                principalTable: "DnsPacket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
