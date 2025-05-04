using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBitcoinBlockHeaders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BitcoinBlockHeader",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    BlockHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PrevBlockHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MerkleRoot = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Bits = table.Column<long>(type: "bigint", nullable: false),
                    Nonce = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinBlockHeader", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinBlockHeader_BlockHash",
                table: "BitcoinBlockHeader",
                column: "BlockHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacketHeader_BitcoinBlockHeaderId",
                table: "BitcoinPacketHeader",
                column: "BitcoinBlockHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitcoinPacketHeader");

            migrationBuilder.DropTable(
                name: "BitcoinBlockHeader");
        }
    }
}
