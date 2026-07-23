namespace EcommerceCheckout.Web.Models.ViewModels;

public class CheckoutPageViewModel
{
    public UserInfoInputModel UserInfo { get; set; } = null!;
    public CartSummary Cart { get; set; } = null!;
}