using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSenderRecipientToDnsPacket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "DnsPacket",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderId",
                table: "DnsPacket",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_RecipientId",
                table: "DnsPacket",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_DnsPacket_SenderId",
                table: "DnsPacket",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DnsPacket_TrafficParticipant_RecipientId",
                table: "DnsPacket",
                column: "RecipientId",
                principalTable: "TrafficParticipant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DnsPacket_TrafficParticipant_SenderId",
                table: "DnsPacket",
                column: "SenderId",
                principalTable: "TrafficParticipant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DnsPacket_TrafficParticipant_RecipientId",
                table: "DnsPacket");

            migrationBuilder.DropForeignKey(
                name: "FK_DnsPacket_TrafficParticipant_SenderId",
                table: "DnsPacket");

            migrationBuilder.DropIndex(
                name: "IX_DnsPacket_RecipientId",
                table: "DnsPacket");

            migrationBuilder.DropIndex(
                name: "IX_DnsPacket_SenderId",
                table: "DnsPacket");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "DnsPacket");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "DnsPacket");
        }
    }
}
