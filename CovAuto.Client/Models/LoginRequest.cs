using System.ComponentModel.DataAnnotations;

namespace CovAuto.Client.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Gebruikersnaam is verplicht")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Wachtwoord is verplicht")]
    public string Password { get; set; } = string.Empty;
}
