using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class update10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Suppliers_SupplierDetailsSupplierId",
                table: "Drugs");

            migrationBuilder.DropIndex(
                name: "IX_Drugs_SupplierDetailsSupplierId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "SupplierDetailsSupplierId",
                table: "Drugs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierDetailsSupplierId",
                table: "Drugs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_SupplierDetailsSupplierId",
                table: "Drugs",
                column: "SupplierDetailsSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Suppliers_SupplierDetailsSupplierId",
                table: "Drugs",
                column: "SupplierDetailsSupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");
        }
    }
}
