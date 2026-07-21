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
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    
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

        modelBuilder.Entity<Order>(e =>
        {
            e.Property(o => o.SubtotalAmount).HasPrecision(18, 2);
            e.Property(o => o.DiscountAmount).HasPrecision(18, 2);
            e.Property(o => o.TotalAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.Property(oi => oi.UnitPrice).HasPrecision(18, 2);
            e.Property(oi => oi.LineTotal).HasPrecision(18, 2);
        });
        
        modelBuilder.Entity<PaymentTransaction>()
                .Property(pt => pt.Amount)
                .HasPrecision(18, 2);
        
        modelBuilder.Entity<PaymentTransaction>()
            .HasOne(i => i.Order)
            .WithOne()
            .HasForeignKey<PaymentTransaction>(i => i.OrderId);

        modelBuilder.Entity<CouponProduct>(e =>
        {
            e.HasKey(cp => new { cp.ProductId, cp.CouponId });

            e.HasOne(cp => cp.Coupon)
                .WithMany(c => c.CouponProducts)
                .HasForeignKey(cp => cp.CouponId);

            e.HasOne(cp => cp.Product)
                .WithMany(c => c.CouponProducts)
                .HasForeignKey(cp => cp.ProductId);
        });
    }
}
