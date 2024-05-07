using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryApp.Migrations
{
    /// <inheritdoc />
    public partial class CartDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Asortiment_AsortimetId",
                table: "CartDetail");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "GoodsStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AsortimetId",
                table: "CartDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Asortiment_AsortimetId",
                table: "CartDetail",
                column: "AsortimetId",
                principalTable: "Asortiment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Asortiment_AsortimetId",
                table: "CartDetail");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "GoodsStatus");

            migrationBuilder.AlterColumn<int>(
                name: "AsortimetId",
                table: "CartDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Asortiment_AsortimetId",
                table: "CartDetail",
                column: "AsortimetId",
                principalTable: "Asortiment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
