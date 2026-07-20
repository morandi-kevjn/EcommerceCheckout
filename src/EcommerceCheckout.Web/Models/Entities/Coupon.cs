namespace EcommerceCheckout.Web.Models.Entities;

public enum DiscountType
{
    Percentage = 1,
    FixedAmount = 2
}

public class Coupon
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public bool Active { get; set; }
    
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    
    public decimal MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}