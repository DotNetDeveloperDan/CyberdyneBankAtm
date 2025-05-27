using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Accounts;

public static class AccountErrors
{
    public static Error NotFound(int accountId)
    {
        return Error.NotFound(
            "Accounts.NotFound",
            $"The account with the Id = '{accountId}' was not found");
    }

    public static Error DatabaseError() =>
        Error.Problem(
            "Accounts.DatabaseError",
            "A database error occurred while retrieving the accounts.");

    public static Error OperationCanceled() =>
        Error.Failure(
            "Accounts.OperationCanceled",
            "The operation to retrieve accounts was canceled.");

    public static Error UnknownError() =>
        Error.Failure(
            "Accounts.UnknownError",
            "An unexpected error occurred while retrieving the accounts.");

    public static Error MultipleFound(int accountId) =>
        Error.Failure(
            "Accounts.MultipleFoundById",
            $"Multiple accounts were found with the Id = '{accountId}'.");
}
