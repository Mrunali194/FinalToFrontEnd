using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class update9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Suppliers_SupplierId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReports_Orders_OrderId",
                table: "SalesReports");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "SalesReports",
                newName: "DrugId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesReports_OrderId",
                table: "SalesReports",
                newName: "IX_SalesReports_DrugId");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Drugs",
                newName: "SupplierDetailsSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_SupplierId",
                table: "Drugs",
                newName: "IX_Drugs_SupplierDetailsSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Suppliers_SupplierDetailsSupplierId",
                table: "Drugs",
                column: "SupplierDetailsSupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReports_Drugs_DrugId",
                table: "SalesReports",
                column: "DrugId",
                principalTable: "Drugs",
                principalColumn: "DrugId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Suppliers_SupplierDetailsSupplierId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReports_Drugs_DrugId",
                table: "SalesReports");

            migrationBuilder.RenameColumn(
                name: "DrugId",
                table: "SalesReports",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesReports_DrugId",
                table: "SalesReports",
                newName: "IX_SalesReports_OrderId");

            migrationBuilder.RenameColumn(
                name: "SupplierDetailsSupplierId",
                table: "Drugs",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_SupplierDetailsSupplierId",
                table: "Drugs",
                newName: "IX_Drugs_SupplierId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Suppliers_SupplierId",
                table: "Drugs",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReports_Orders_OrderId",
                table: "SalesReports",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
