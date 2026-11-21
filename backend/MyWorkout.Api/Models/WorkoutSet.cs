namespace MyWorkout.Api.Models;

// Jedna série cviku. Třída se jmenuje WorkoutSet, tabulka v databázi Sets.
public class WorkoutSet
{
    public int Id { get; set; }
    public int WorkoutExerciseId { get; set; }
    public int SetNumber { get; set; }
    public int Reps { get; set; }
    public decimal WeightKg { get; set; }

    public WorkoutExercise? WorkoutExercise { get; set; }
}
