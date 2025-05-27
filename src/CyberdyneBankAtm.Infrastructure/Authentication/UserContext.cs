using CyberdyneBankAtm.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace CyberdyneBankAtm.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId()??
       1;
}