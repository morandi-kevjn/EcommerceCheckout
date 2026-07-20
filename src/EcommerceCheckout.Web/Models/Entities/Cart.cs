namespace EcommerceCheckout.Web.Models.Entities;

public enum CartStatus
{
    Open = 1,
    Converted = 2,
    Abandoned = 3
}

public class Cart
{
    public int Id { get; set; }
    public Guid CartToken { get; set; } = Guid.NewGuid();
    
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    public CartStatus Status { get; set; } = CartStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}

public class CartItem
{
    public int Id { get; set; }
    
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}