using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFileAnalysisEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrafficParticipant_StoredFile_StoredFileId",
                table: "TrafficParticipant");

            migrationBuilder.RenameColumn(
                name: "StoredFileId",
                table: "TrafficParticipant",
                newName: "FileAnalysisId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficParticipant_StoredFileId",
                table: "TrafficParticipant",
                newName: "IX_TrafficParticipant_FileAnalysisId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficParticipant_Address_Port_StoredFileId",
                table: "TrafficParticipant",
                newName: "IX_TrafficParticipant_Address_Port_FileAnalysisId");

            migrationBuilder.CreateTable(
                name: "FileAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StoredFileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileAnalysis_StoredFile_StoredFileId",
                        column: x => x.StoredFileId,
                        principalTable: "StoredFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileAnalysis_StoredFileId",
                table: "FileAnalysis",
                column: "StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficParticipant_FileAnalysis_FileAnalysisId",
                table: "TrafficParticipant",
                column: "FileAnalysisId",
                principalTable: "FileAnalysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrafficParticipant_FileAnalysis_FileAnalysisId",
                table: "TrafficParticipant");

            migrationBuilder.DropTable(
                name: "FileAnalysis");

            migrationBuilder.RenameColumn(
                name: "FileAnalysisId",
                table: "TrafficParticipant",
                newName: "StoredFileId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficParticipant_FileAnalysisId",
                table: "TrafficParticipant",
                newName: "IX_TrafficParticipant_StoredFileId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficParticipant_Address_Port_FileAnalysisId",
                table: "TrafficParticipant",
                newName: "IX_TrafficParticipant_Address_Port_StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficParticipant_StoredFile_StoredFileId",
                table: "TrafficParticipant",
                column: "StoredFileId",
                principalTable: "StoredFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
