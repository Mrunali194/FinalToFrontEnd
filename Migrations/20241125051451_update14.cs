using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class update14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReports_Drugs_DrugId",
                table: "SalesReports");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Drugs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_SupplierId",
                table: "Drugs",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Suppliers_SupplierId",
                table: "Drugs",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReports_Drugs_DrugId",
                table: "SalesReports",
                column: "DrugId",
                principalTable: "Drugs",
                principalColumn: "DrugId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Suppliers_SupplierId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReports_Drugs_DrugId",
                table: "SalesReports");

            migrationBuilder.DropIndex(
                name: "IX_Drugs_SupplierId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Drugs");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReports_Drugs_DrugId",
                table: "SalesReports",
                column: "DrugId",
                principalTable: "Drugs",
                principalColumn: "DrugId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
