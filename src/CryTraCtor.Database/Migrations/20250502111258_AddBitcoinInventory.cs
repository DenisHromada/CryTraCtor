using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBitcoinInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BitcoinInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinInventory", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_BitcoinPacketInventory_BitcoinInventoryId",
                table: "BitcoinPacketInventory",
                column: "BitcoinInventoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitcoinPacketInventory");

            migrationBuilder.DropTable(
                name: "BitcoinInventory");
        }
    }
}
