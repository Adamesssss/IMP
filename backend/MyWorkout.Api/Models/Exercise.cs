namespace MyWorkout.Api.Models;

// Jeden cvik ze sdílené databáze cviků (stejná pro všechny uživatele).
public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string MuscleGroup { get; set; } = "";
    public string? Equipment { get; set; }
    public string? Difficulty { get; set; }
    public string? Description { get; set; }
    public int? Rating { get; set; }
    public string? Source { get; set; }
}
