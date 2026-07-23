using System.ComponentModel.DataAnnotations;

namespace EcommerceCheckout.Web.Models.ViewModels;

public class UserInfoInputModel
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }  = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Nation is required")]
    public string Nation { get; set; } = string.Empty;

    public bool Newsletter { get; set; } = false;
    public bool Invoice { get; set; } = false;
    
    public string? FiscalTaxNumber { get; set; }
    public string? FiscalCodeNUmber { get; set; }
    
    [Required(ErrorMessage = "Privacy Policy is required")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Privacy Policy is invalid")]
    public bool PrivacyAccepted { get; set; }
}