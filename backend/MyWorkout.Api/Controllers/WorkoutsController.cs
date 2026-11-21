using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWorkout.Api.Data;
using MyWorkout.Api.Dtos;
using MyWorkout.Api.Models;
using MyWorkout.Api.Services;

namespace MyWorkout.Api.Controllers;

[ApiController]
[Route("api/workouts")]
[Authorize]
public class WorkoutsController : ControllerBase
{
    private readonly AppDbContext _db;

    public WorkoutsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/workouts – moje tréninky od nejnovějšího
    [HttpGet]
    public async Task<List<WorkoutListItemDto>> GetMine()
    {
        var userId = User.GetUserId();

        return await _db.Workouts
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Date)
            .ThenByDescending(w => w.Id)
            .Select(w => new WorkoutListItemDto
            {
                Id = w.Id,
                Date = w.Date,
                Name = w.Name,
                DurationMin = w.DurationMin,
                ExerciseCount = w.WorkoutExercises.Count,
                SetCount = w.WorkoutExercises.SelectMany(we => we.Sets).Count(),
                VolumeKg = w.WorkoutExercises.SelectMany(we => we.Sets).Sum(s => s.Reps * s.WeightKg),
            })
            .ToListAsync();
    }

    // GET /api/workouts/5 – jeden můj trénink se všemi cviky a sériemi
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkoutDetailDto>> GetById(int id)
    {
        var userId = User.GetUserId();

        var workout = await _db.Workouts
            .Include(w => w.WorkoutExercises).ThenInclude(we => we.Exercise)
            .Include(w => w.WorkoutExercises).ThenInclude(we => we.Sets)
            .SingleOrDefaultAsync(w => w.Id == id && w.UserId == userId);

        if (workout == null)
            return NotFound();

        return ToDetail(workout);
    }

    // POST /api/workouts – uložení nového tréninku
    [HttpPost]
    public async Task<ActionResult<WorkoutDetailDto>> Create(CreateWorkoutRequest request)
    {
        var userId = User.GetUserId();

        // Všechny vybrané cviky musí existovat v databázi cviků.
        var exerciseIds = request.Exercises.Select(e => e.ExerciseId).Distinct().ToList();
        var foundCount = await _db.Exercises.CountAsync(e => exerciseIds.Contains(e.Id));
        if (foundCount != exerciseIds.Count)
        {
            ModelState.AddModelError(nameof(request.Exercises), "Některý z vybraných cviků neexistuje.");
            return ValidationProblem(ModelState);
        }

        var workout = new Workout
        {
            UserId = userId,
            Date = request.Date!.Value,
            Name = string.IsNullOrWhiteSpace(request.Name) ? null : request.Name.Trim(),
            Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim(),
            DurationMin = request.DurationMin,
        };

        // Pořadí cviků a čísla sérií dopočítá server podle pořadí v požadavku.
        for (var i = 0; i < request.Exercises.Count; i++)
        {
            var exerciseRequest = request.Exercises[i];
            var workoutExercise = new WorkoutExercise
            {
                ExerciseId = exerciseRequest.ExerciseId,
                OrderIndex = i + 1,
            };

            for (var j = 0; j < exerciseRequest.Sets.Count; j++)
            {
                workoutExercise.Sets.Add(new WorkoutSet
                {
                    SetNumber = j + 1,
                    Reps = exerciseRequest.Sets[j].Reps!.Value,
                    WeightKg = exerciseRequest.Sets[j].WeightKg!.Value,
                });
            }

            workout.WorkoutExercises.Add(workoutExercise);
        }

        _db.Workouts.Add(workout);
        await _db.SaveChangesAsync();

        // Načteme trénink znovu i s názvy cviků a vrátíme ho (201 Created).
        var saved = await _db.Workouts
            .Include(w => w.WorkoutExercises).ThenInclude(we => we.Exercise)
            .Include(w => w.WorkoutExercises).ThenInclude(we => we.Sets)
            .SingleAsync(w => w.Id == workout.Id);

        return CreatedAtAction(nameof(GetById), new { id = saved.Id }, ToDetail(saved));
    }

    private static WorkoutDetailDto ToDetail(Workout w)
    {
        var exercises = w.WorkoutExercises
            .OrderBy(we => we.OrderIndex)
            .Select(we => new WorkoutExerciseDto
            {
                ExerciseId = we.ExerciseId,
                Name = we.Exercise!.Name,
                MuscleGroup = we.Exercise.MuscleGroup,
                Equipment = we.Exercise.Equipment,
                OrderIndex = we.OrderIndex,
                Sets = we.Sets
                    .OrderBy(s => s.SetNumber)
                    .Select(s => new SetDto { SetNumber = s.SetNumber, Reps = s.Reps, WeightKg = s.WeightKg })
                    .ToList(),
            })
            .ToList();

        return new WorkoutDetailDto
        {
            Id = w.Id,
            Date = w.Date,
            Name = w.Name,
            Note = w.Note,
            DurationMin = w.DurationMin,
            VolumeKg = exercises.SelectMany(e => e.Sets).Sum(s => s.Reps * s.WeightKg),
            Exercises = exercises,
        };
    }
}
