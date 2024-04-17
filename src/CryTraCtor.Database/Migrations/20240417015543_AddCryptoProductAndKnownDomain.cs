using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryTraCtor.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCryptoProductAndKnownDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Vendor = table.Column<string>(type: "text", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnownDomain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DomainName = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    KnownProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CryptoProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnownDomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnownDomain_CryptoProduct_CryptoProductId",
                        column: x => x.CryptoProductId,
                        principalTable: "CryptoProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KnownDomain_CryptoProductId",
                table: "KnownDomain",
                column: "CryptoProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KnownDomain");

            migrationBuilder.DropTable(
                name: "CryptoProduct");
        }
    }
}
