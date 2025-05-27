using System.Security.Claims;

namespace CyberdyneBankAtm.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new ApplicationException("User id is unavailable");
    }
}