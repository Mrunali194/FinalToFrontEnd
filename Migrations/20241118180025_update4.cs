using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Drugs_DrugDetailsDrugId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DrugDetailsDrugId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DrugDetailsDrugId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DrugDetailsDrugId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DrugDetailsDrugId",
                table: "Orders",
                column: "DrugDetailsDrugId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Drugs_DrugDetailsDrugId",
                table: "Orders",
                column: "DrugDetailsDrugId",
                principalTable: "Drugs",
                principalColumn: "DrugId");
        }
    }
}
