using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWorkout.Api.Data;
using MyWorkout.Api.Dtos;

namespace MyWorkout.Api.Controllers;

[ApiController]
[Route("api/exercises")]
[Authorize]
public class ExercisesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ExercisesController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/exercises – seznam cviků pro výběr v editoru tréninku
    [HttpGet]
    public async Task<List<ExerciseDto>> GetAll()
    {
        return await _db.Exercises
            .OrderBy(e => e.MuscleGroup)
            .ThenBy(e => e.Name)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                MuscleGroup = e.MuscleGroup,
                Equipment = e.Equipment,
                Difficulty = e.Difficulty,
                Description = e.Description,
            })
            .ToListAsync();
    }
}
