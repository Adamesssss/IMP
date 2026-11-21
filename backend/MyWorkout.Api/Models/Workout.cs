namespace MyWorkout.Api.Models;

// Jeden odcvičený trénink. Patří vždy jednomu uživateli (UserId).
public class Workout
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly Date { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public int? DurationMin { get; set; }

    public User? User { get; set; }
    public List<WorkoutExercise> WorkoutExercises { get; set; } = new();
}
