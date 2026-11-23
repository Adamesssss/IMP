using System.ComponentModel.DataAnnotations;

namespace MyWorkout.Api.Dtos;

//Co posílá frontend při uložení tréninku 

public class CreateWorkoutRequest
{
    [Required(ErrorMessage = "Zadej datum tréninku.")]
    public DateOnly? Date { get; set; }

    [MaxLength(100, ErrorMessage = "Název může mít nejvýš 100 znaků.")]
    public string? Name { get; set; }

    [MaxLength(1000, ErrorMessage = "Poznámka může mít nejvýš 1000 znaků.")]
    public string? Note { get; set; }

    [Range(1, 600, ErrorMessage = "Délka musí být 1 až 600 minut.")]
    public int? DurationMin { get; set; }

    [MinLength(1, ErrorMessage = "Trénink musí mít aspoň jeden cvik.")]
    public List<CreateWorkoutExerciseRequest> Exercises { get; set; } = new();
}

public class CreateWorkoutExerciseRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Vyber cvik.")]
    public int ExerciseId { get; set; }

    [MinLength(1, ErrorMessage = "Každý cvik musí mít aspoň jednu sérii.")]
    public List<CreateSetRequest> Sets { get; set; } = new();
}

public class CreateSetRequest
{
    [Required(ErrorMessage = "Zadej počet opakování.")]
    [Range(1, 1000, ErrorMessage = "Opakování musí být větší než 0.")]
    public int? Reps { get; set; }

    [Required(ErrorMessage = "Zadej váhu (0 = vlastní váha).")]
    [Range(0.0, 1000.0, ErrorMessage = "Váha musí být 0 až 1000 kg.")]
    public decimal? WeightKg { get; set; }
}

//Co backend vrací

// Řádek v seznamu tréninků.
public class WorkoutListItemDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string? Name { get; set; }
    public int? DurationMin { get; set; }
    public int ExerciseCount { get; set; }
    public int SetCount { get; set; }
    public decimal VolumeKg { get; set; }
}

// Celý trénink včetně cviků a sérií.
public class WorkoutDetailDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public int? DurationMin { get; set; }
    public decimal VolumeKg { get; set; }
    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}

public class WorkoutExerciseDto
{
    public int ExerciseId { get; set; }
    public string Name { get; set; } = "";
    public string MuscleGroup { get; set; } = "";
    public string? Equipment { get; set; }
    public int OrderIndex { get; set; }
    public List<SetDto> Sets { get; set; } = new();
}

public class SetDto
{
    public int SetNumber { get; set; }
    public int Reps { get; set; }
    public decimal WeightKg { get; set; }
}
