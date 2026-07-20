using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceCheckout.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Customers_InvoiceRequiresFiscalData",
                table: "Customers",
                sql: "[RequiresInvoice] = 0 OR [FiscalTaxNumber] IS NOT NULL OR [FiscalCodeNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Customers_InvoiceRequiresFiscalData",
                table: "Customers");
        }
    }
}
