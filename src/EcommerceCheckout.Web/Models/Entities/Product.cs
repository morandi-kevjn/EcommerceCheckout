namespace EcommerceCheckout.Web.Models.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? SKU { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    public bool IsAvailable { get; set; }
    public bool Active { get; set; } = true;
    
    public byte[]? RowVersion { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<CouponProduct> CouponProducts { get; set; } = new List<CouponProduct>();
}