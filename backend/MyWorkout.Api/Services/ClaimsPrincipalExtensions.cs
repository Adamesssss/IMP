using System.Security.Claims;

namespace MyWorkout.Api.Services;

public static class ClaimsPrincipalExtensions
{
    // Přečte Id přihlášeného uživatele z ověřeného tokenu (claim "sub").
    public static int GetUserId(this ClaimsPrincipal user)
    {
        return int.Parse(user.FindFirstValue("sub")!);
    }
}
