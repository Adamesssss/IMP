namespace MyWorkout.Api.Dtos;

public class ExerciseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string MuscleGroup { get; set; } = "";
    public string? Equipment { get; set; }
    public string? Difficulty { get; set; }
    public string? Description { get; set; }
}
