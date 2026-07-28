using System.ComponentModel.DataAnnotations;

namespace EcommerceCheckout.Web.Models.ViewModels;

public class UserInfoInputModel
{
    [Required(ErrorMessage = "Il nome é obbligatorio")]
    [Display(Name = "Nome")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Il cognome é obbligatorio")]
    [Display(Name = "Cognome")]
    public string LastName { get; set; }  = string.Empty;
    
    [Required(ErrorMessage = "L'email é obbligatoria")]
    [EmailAddress(ErrorMessage = "Indirizzo Email non valido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La nazione é obbligatoria")]
    [Display(Name = "Nazione")]
    public string Nation { get; set; } = string.Empty;
    
    [Display(Name = "Iscriviti alla newsletter")]
    public bool Newsletter { get; set; } = false;
    
    [Display(Name = "Iscriviti alla newsletter")]
    public bool Invoice { get; set; } = false;
    
    [Display(Name = "Partita IVA")]
    public string? FiscalTaxNumber { get; set; }
    
    [Display(Name = "Codice Fiscale")]
    public string? FiscalCodeNumber { get; set; }
    
    [Required(ErrorMessage = "La policy sulla privacy é obbligatoria")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Devi accettare l'informativa sulla privacy")]
    [Display(Name = "Privacy")]
    public bool PrivacyAccepted { get; set; }
}