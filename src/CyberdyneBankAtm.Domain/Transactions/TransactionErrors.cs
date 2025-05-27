using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Transactions;

public static class TransactionErrors
{
    public static Error NotFound(int transactionId)
    {
        return Error.NotFound(
            "Transaction.NotFound",
            $"The transaction with the Id = '{transactionId}' was not found");
    }

    public static Error AccountNotFound(int accountId)
    {
        return Error.NotFound(
            "Transaction.Account.NotFound",
            $"The account with the Id = '{accountId}' was not found");
    }

    public static Error InsufficientFunds(int accountId)
    {
        return Error.Problem(
            "Transaction.Account.Insf",
            $"The account with the Id = '{accountId}' does not have sufficient funds");
    }
    public static Error ConcurrencyError() =>
        Error.Problem(
            "Transaction.ConcurrencyError",
            "A concurrency conflict occurred while processing the transaction.");

    public static Error DatabaseError() =>
        Error.Problem(
            "Transaction.DatabaseError",
            "A database error occurred while processing the transaction.");

    public static Error EventPublishError() =>
        Error.Problem(
            "Transaction.EventPublishError",
            "An error occurred while publishing domain events for the transaction.");

    public static Error OperationCanceled() =>
        Error.Problem(
            "Transaction.OperationCanceled",
            "The transaction operation was canceled.");

    public static Error UnknownError() =>
        Error.Problem(
            "Transaction.UnknownError",
            "An unexpected error occurred while processing the transaction.");
}