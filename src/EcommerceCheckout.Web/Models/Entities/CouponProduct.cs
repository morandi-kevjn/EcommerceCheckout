namespace EcommerceCheckout.Web.Models.Entities;

public class CouponProduct
{
    public int CouponId { get; set; }
    public Coupon Coupon { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}