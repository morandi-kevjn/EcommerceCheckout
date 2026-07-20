using EcommerceCheckout.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18,2);

        modelBuilder.Entity<Coupon>(e =>
        {
            e.Property(c => c.DiscountValue).HasPrecision(18, 2);
            e.Property(c => c.MinPrice).HasPrecision(18, 2);
            e.Property(c => c.MaxPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable(t => t.HasCheckConstraint(
                "CK_Customers_InvoiceRequiresFiscalData",
                "[RequiresInvoice] = 0 OR [FiscalTaxNumber] IS NOT NULL OR [FiscalCodeNumber] IS NOT NULL"));
        });
        
        modelBuilder.Entity<CartItem>()
            .Property(ci => ci.UnitPrice)
            .HasPrecision(18, 2);
    }
}
