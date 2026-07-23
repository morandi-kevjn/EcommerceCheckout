using System.ComponentModel.DataAnnotations;

namespace EcommerceCheckout.Web.Models.ViewModels;

public class UserInfoInputModel
{
    [Required(ErrorMessage = "Il nome é obbligatorio")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Il cognome é obbligatorio")]
    public string LastName { get; set; }  = string.Empty;
    
    [Required(ErrorMessage = "L'email é obbligatoria")]
    [EmailAddress(ErrorMessage = "Indirizzo Email non valido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La nazione é obbligatoria")]
    public string Nation { get; set; } = string.Empty;

    public bool Newsletter { get; set; } = false;
    public bool Invoice { get; set; } = false;
    
    public string? FiscalTaxNumber { get; set; }
    public string? FiscalCodeNUmber { get; set; }
    
    [Required(ErrorMessage = "La policy sulla privacy é obbligatoria")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Devi accettare l'informativa sulla privacy")]
    public bool PrivacyAccepted { get; set; }
}