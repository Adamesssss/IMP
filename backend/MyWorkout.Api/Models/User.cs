namespace MyWorkout.Api.Models;

// Jeden řádek tabulky Users = jeden účet v aplikaci.
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Role { get; set; } = "user";

    // Nepovinné údaje z profilu (otazník = může být prázdné, v databázi NULL).
    public int? HeightCm { get; set; }
    public decimal? WeightKg { get; set; }
    public string? Level { get; set; }
    public string? Goal { get; set; }

    public DateTime PrivacyConsentAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigační vlastnost: všechny tréninky tohoto uživatele.
    public List<Workout> Workouts { get; set; } = new();
}
