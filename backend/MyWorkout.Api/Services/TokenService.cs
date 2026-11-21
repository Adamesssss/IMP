using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyWorkout.Api.Models;

namespace MyWorkout.Api.Services;

// Vyrábí JWT token po úspěšné registraci nebo přihlášení.
public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public (string Token, DateTime ExpiresAt) CreateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var expiresAt = DateTime.UtcNow.AddHours(_config.GetValue("Jwt:ExpiresHours", 24));

        var descriptor = new SecurityTokenDescriptor
        {
            // Údaje (claims), které budou v tokenu.
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("name", user.DisplayName),
                new Claim("role", user.Role),
            }),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            Expires = expiresAt,
            // Podpis tajným klíčem algoritmem HMAC SHA-256.
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return (token, expiresAt);
    }
}
