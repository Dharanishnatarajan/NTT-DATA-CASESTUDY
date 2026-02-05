using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReorderLevel = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_ItemName",
                table: "InventoryItems",
                column: "ItemName");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_Quantity",
                table: "InventoryItems",
                column: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");
        }
    }
}
