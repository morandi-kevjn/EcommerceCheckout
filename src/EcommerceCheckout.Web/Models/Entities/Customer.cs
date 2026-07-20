namespace EcommerceCheckout.Web.Models.Entities;

public class Customer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Nation { get; set; }
    
    public bool NewsletterOptIn { get; set; }
    public bool RequiresInvoice { get; set; }

    public string? FiscalTaxNumber { get; set; }
    public string? FiscalCodeNumber { get; set; }
    
    public DateTime PrivacyAcceptedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}