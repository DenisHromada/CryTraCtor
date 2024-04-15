using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class StoredFilePublicAndPrivateFilePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "StoredFiles",
                newName: "PublicFileName");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "StoredFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "InternalFilePath",
                table: "StoredFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "StoredFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "StoredFiles");

            migrationBuilder.DropColumn(
                name: "InternalFilePath",
                table: "StoredFiles");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "StoredFiles");

            migrationBuilder.RenameColumn(
                name: "PublicFileName",
                table: "StoredFiles",
                newName: "FilePath");
        }
    }
}
