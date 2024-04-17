using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class EnforceStoredFilePublicNameIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StoredFile_PublicFileName",
                table: "StoredFile",
                column: "PublicFileName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StoredFile_PublicFileName",
                table: "StoredFile");
        }
    }
}
