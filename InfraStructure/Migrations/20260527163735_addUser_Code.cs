using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class addUser_Code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Code",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User_Code",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User_Code",
                table: "Order_Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User_Code",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User_Code",
                table: "categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_Main_Category",
                table: "categories",
                column: "Main_Category");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_Main_Category",
                table: "categories",
                column: "Main_Category",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_Main_Category",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_Main_Category",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "User_Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "User_Code",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "User_Code",
                table: "Order_Items");

            migrationBuilder.DropColumn(
                name: "User_Code",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "User_Code",
                table: "categories");
        }
    }
}
