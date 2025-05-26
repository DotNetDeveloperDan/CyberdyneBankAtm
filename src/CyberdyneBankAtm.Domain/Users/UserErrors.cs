using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static Error NotFound(Guid userId)
    {
        return Error.NotFound(
            "Users.NotFound",
            $"The user with the Id = '{userId}' was not found");
    }

    public static Error Unauthorized()
    {
        return Error.Failure(
            "Users.Unauthorized",
            "You are not authorized to perform this action.");
    }
}