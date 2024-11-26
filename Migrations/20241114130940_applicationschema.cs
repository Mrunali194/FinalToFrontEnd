using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class applicationschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "supplierDetails",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplierDetails", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "userDetailsDb",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDetailsDb", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "inventoryDetails",
                columns: table => new
                {
                    DrugId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventoryDetails", x => x.DrugId);
                    table.ForeignKey(
                        name: "FK_inventoryDetails_supplierDetails_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "supplierDetails",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orderDetails",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderDetails", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_orderDetails_userDetailsDb_UserId",
                        column: x => x.UserId,
                        principalTable: "userDetailsDb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drugCartDb",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drugCartDb", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_drugCartDb_inventoryDetails_DrugId",
                        column: x => x.DrugId,
                        principalTable: "inventoryDetails",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_drugCartDb_userDetailsDb_UserId",
                        column: x => x.UserId,
                        principalTable: "userDetailsDb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "drugSalesReportDb",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AmountSold = table.Column<int>(type: "int", nullable: false),
                    SaleAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drugSalesReportDb", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_drugSalesReportDb_orderDetails_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orderDetails",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDetailsOrderDetails",
                columns: table => new
                {
                    InventoriesDrugId = table.Column<int>(type: "int", nullable: false),
                    OrderDetailsOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDetailsOrderDetails", x => new { x.InventoriesDrugId, x.OrderDetailsOrderId });
                    table.ForeignKey(
                        name: "FK_InventoryDetailsOrderDetails_inventoryDetails_InventoriesDrugId",
                        column: x => x.InventoriesDrugId,
                        principalTable: "inventoryDetails",
                        principalColumn: "DrugId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryDetailsOrderDetails_orderDetails_OrderDetailsOrderId",
                        column: x => x.OrderDetailsOrderId,
                        principalTable: "orderDetails",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactionDetails",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactionDetails", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_transactionDetails_drugCartDb_CartId",
                        column: x => x.CartId,
                        principalTable: "drugCartDb",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactionDetails_orderDetails_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orderDetails",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_drugCartDb_DrugId",
                table: "drugCartDb",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_drugCartDb_UserId",
                table: "drugCartDb",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_drugSalesReportDb_OrderId",
                table: "drugSalesReportDb",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventoryDetails_SupplierId",
                table: "inventoryDetails",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetailsOrderDetails_OrderDetailsOrderId",
                table: "InventoryDetailsOrderDetails",
                column: "OrderDetailsOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_UserId",
                table: "orderDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_transactionDetails_CartId",
                table: "transactionDetails",
                column: "CartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactionDetails_OrderId",
                table: "transactionDetails",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drugSalesReportDb");

            migrationBuilder.DropTable(
                name: "InventoryDetailsOrderDetails");

            migrationBuilder.DropTable(
                name: "transactionDetails");

            migrationBuilder.DropTable(
                name: "drugCartDb");

            migrationBuilder.DropTable(
                name: "orderDetails");

            migrationBuilder.DropTable(
                name: "inventoryDetails");

            migrationBuilder.DropTable(
                name: "userDetailsDb");

            migrationBuilder.DropTable(
                name: "supplierDetails");
        }
    }
}
