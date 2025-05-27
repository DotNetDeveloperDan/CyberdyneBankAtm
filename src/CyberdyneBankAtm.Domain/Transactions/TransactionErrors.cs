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
}