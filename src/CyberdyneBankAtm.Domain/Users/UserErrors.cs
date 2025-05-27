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

    public static Error NotFound(int userId)
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
    public static Error MultipleUsersFound(string email) =>
        Error.Failure(
            "Users.MultipleFoundByEmail",
            $"Multiple users were found with the email '{email}'.");

    public static Error DatabaseError() =>
        Error.Failure(
            "Users.DatabaseError",
            "A database error occurred while retrieving the user.");

    public static Error OperationCanceled() =>
        Error.Failure(
            "Users.OperationCanceled",
            "The operation was canceled before completion.");

    public static Error UnknownError() =>
        Error.Failure(
            "Users.UnknownError",
            "An unexpected error occurred while retrieving the user.");
}