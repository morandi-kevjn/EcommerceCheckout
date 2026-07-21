namespace EcommerceCheckout.Web.Models.Entities;

public class PaymentTransaction
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public PaymentProviderType Provider { get; set; }
    public string? ProviderTransactionId { get; set; }
    public string Status { get; set; } = "created";
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    
    public string? RawResponse { get; set; }
    
    public DateTime CreatedAt { get; set; }
}