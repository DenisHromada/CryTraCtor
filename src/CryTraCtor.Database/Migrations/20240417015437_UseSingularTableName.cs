using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class UseSingularTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredFiles");

            migrationBuilder.CreateTable(
                name: "StoredFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicFileName = table.Column<string>(type: "text", nullable: false),
                    InternalFilePath = table.Column<string>(type: "text", nullable: false),
                    MimeType = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFile", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredFile");

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    InternalFilePath = table.Column<string>(type: "text", nullable: false),
                    MimeType = table.Column<string>(type: "text", nullable: false),
                    PublicFileName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Id);
                });
        }
    }
}
