using EcommerceCheckout.Web.Models.Entities;

namespace EcommerceCheckout.Web.Models.ViewModels;

public class CartSummary
{
    public Cart Cart { get; set; } = null!;
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total => Subtotal - Discount;
}