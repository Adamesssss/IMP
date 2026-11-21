namespace MyWorkout.Api.Models;

// Spojovací tabulka: který cvik byl v kterém tréninku a v jakém pořadí.
public class WorkoutExercise
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int ExerciseId { get; set; }
    public int OrderIndex { get; set; }

    public Workout? Workout { get; set; }
    public Exercise? Exercise { get; set; }
    public List<WorkoutSet> Sets { get; set; } = new();
}
