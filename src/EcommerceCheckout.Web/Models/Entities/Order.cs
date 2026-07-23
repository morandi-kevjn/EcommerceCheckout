namespace EcommerceCheckout.Web.Models.Entities;

public enum OrderStatus
{
    PendingPayment = 1,
    Paid = 2,
    Failed = 3,
    Cancelled = 4
}

public enum PaymentProviderType
{
    Stripe = 1,
    PayPal = 2
}

public class Order
{
    public int Id { get; set; }
    public required string OrderNumber { get; set; }
    
    public int CustomerId { get; set; }
    public required Customer Customer { get; set; }
    
    public int? CartId { get; set; }
    public Cart? Cart { get; set; }
    
    public int? CouponId { get; set; }
    public Coupon? Coupon { get; set; }
    
    public decimal SubtotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "EUR";

    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
    public PaymentProviderType PaymentProvider { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId  { get; set; }
    public Order Order { get; set; } = null!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public required string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
}