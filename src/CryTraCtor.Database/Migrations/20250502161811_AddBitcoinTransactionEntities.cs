using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBitcoinTransactionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BitcoinTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TxId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Locktime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinTransaction", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "BitcoinTransactionInput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinTransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousTxHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PreviousOutputIndex = table.Column<long>(type: "bigint", nullable: false),
                    ScriptSig = table.Column<string>(type: "text", nullable: false),
                    Sequence = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinTransactionInput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitcoinTransactionInput_BitcoinTransaction_BitcoinTransacti~",
                        column: x => x.BitcoinTransactionId,
                        principalTable: "BitcoinTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinTransactionOutput",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BitcoinTransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    ScriptPubKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinTransactionOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitcoinTransactionOutput_BitcoinTransaction_BitcoinTransact~",
                        column: x => x.BitcoinTransactionId,
                        principalTable: "BitcoinTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacketTransaction_BitcoinTransactionId",
                table: "BitcoinPacketTransaction",
                column: "BitcoinTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinTransaction_TxId",
                table: "BitcoinTransaction",
                column: "TxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinTransactionInput_BitcoinTransactionId",
                table: "BitcoinTransactionInput",
                column: "BitcoinTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinTransactionOutput_BitcoinTransactionId",
                table: "BitcoinTransactionOutput",
                column: "BitcoinTransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitcoinPacketTransaction");

            migrationBuilder.DropTable(
                name: "BitcoinTransactionInput");

            migrationBuilder.DropTable(
                name: "BitcoinTransactionOutput");

            migrationBuilder.DropTable(
                name: "BitcoinTransaction");
        }
    }
}
