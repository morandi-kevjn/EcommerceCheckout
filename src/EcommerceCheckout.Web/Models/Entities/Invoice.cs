namespace EcommerceCheckout.Web.Models.Entities;

public class Invoice
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public required string InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public string? VatNumber { get; set; }
    public string? FiscalCode { get; set; }
    
    public string? PdfBlobPath { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}