using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWorkout.Api.Data;
using MyWorkout.Api.Dtos;
using MyWorkout.Api.Models;
using MyWorkout.Api.Services;

namespace MyWorkout.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokens;

    public AuthController(AppDbContext db, TokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!request.PrivacyConsent)
            ModelState.AddModelError(nameof(request.PrivacyConsent), "Bez souhlasu se zásadami nelze účet založit.");
        if (!request.AgeConfirmed)
            ModelState.AddModelError(nameof(request.AgeConfirmed), "Aplikaci můžeš používat od 15 let.");
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _db.Users.AnyAsync(u => u.Email == email))
            return Conflict(new { message = "E-mail je již registrován." });

        var now = DateTime.UtcNow;
        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DisplayName = request.DisplayName.Trim(),
            Role = "user",
            PrivacyConsentAt = now,
            CreatedAt = now,
        };

        _db.Users.Add(user);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Dva požadavky se stejným e-mailem ve stejnou chvíli: druhý zastaví unikátní index.
            return Conflict(new { message = "E-mail je již registrován." });
        }

        return StatusCode(StatusCodes.Status201Created, CreateAuthResponse(user));
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);

        // Stejná hláška pro neexistující e-mail i špatné heslo (útočník nezjistí, které účty existují).
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Neplatný e-mail nebo heslo." });

        return Ok(CreateAuthResponse(user));
    }

    // GET /api/auth/me – kdo je přihlášený (podle tokenu)
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
    {
        var userId = User.GetUserId();
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
            return Unauthorized();

        return ToDto(user);
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        var (token, expiresAt) = _tokens.CreateToken(user);
        return new AuthResponse { Token = token, ExpiresAt = expiresAt, User = ToDto(user) };
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        DisplayName = user.DisplayName,
        Role = user.Role,
    };
}
