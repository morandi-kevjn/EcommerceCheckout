using EcommerceCheckout.Web.Models.Entities;

namespace EcommerceCheckout.Web.Data;

public class DbInitializer
{
    public static void EnsureSeeded(ApplicationDbContext db)
    {
        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "T-Shirt Test", Price = 19.90m, StockQty = 100 },
                new Product { Name = "Mug Test", Price = 12.50m, StockQty = 50 },
                new Product { Name = "Hoodie Test", Price = 44.00m, StockQty = 30 }            );

            if (!db.Coupons.Any())
            {
                db.Coupons.AddRange(
                    new Coupon { Code = "WELCOME10", DiscountType = DiscountType.Percentage, DiscountValue = 10m, MinPrice = 0 }
                );
            }
            
            db.SaveChanges();
        }
    }
}