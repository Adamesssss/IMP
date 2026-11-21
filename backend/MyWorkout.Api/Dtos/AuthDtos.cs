using System.ComponentModel.DataAnnotations;

namespace MyWorkout.Api.Dtos;

// DTO = data, která chodí mezi frontendem a backendem.
// Atributy v hranatých závorkách kontroluje ASP.NET automaticky,
// při chybě vrátí 400 se seznamem chyb a do metody v controlleru se vůbec nedostane.

public class RegisterRequest
{
    [Required(ErrorMessage = "Zadej e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail nemá správný tvar.")]
    [MaxLength(254)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Zadej heslo.")]
    [RegularExpression(@"^(?=.*\p{Lu})(?=.*\d).{8,100}$",
        ErrorMessage = "Heslo musí mít aspoň 8 znaků, jedno velké písmeno a číslici.")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Zadej jméno.")]
    [MaxLength(50, ErrorMessage = "Jméno může mít nejvýš 50 znaků.")]
    public string DisplayName { get; set; } = "";

    public bool PrivacyConsent { get; set; }
    public bool AgeConfirmed { get; set; }
}

public class LoginRequest
{
    [Required(ErrorMessage = "Zadej e-mail.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Zadej heslo.")]
    public string Password { get; set; } = "";
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Role { get; set; } = "";
}

public class AuthResponse
{
    public string Token { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = new();
}
