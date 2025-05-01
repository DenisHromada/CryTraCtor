using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBitcoinHeaderFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Checksum",
                table: "BitcoinPacket",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Command",
                table: "BitcoinPacket",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "BitcoinPacket",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Magic",
                table: "BitcoinPacket",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checksum",
                table: "BitcoinPacket");

            migrationBuilder.DropColumn(
                name: "Command",
                table: "BitcoinPacket");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "BitcoinPacket");

            migrationBuilder.DropColumn(
                name: "Magic",
                table: "BitcoinPacket");
        }
    }
}
