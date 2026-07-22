using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceCheckout.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCouponToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CouponId",
                table: "Carts",
                column: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Coupons_CouponId",
                table: "Carts",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Coupons_CouponId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CouponId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Carts");
        }
    }
}
