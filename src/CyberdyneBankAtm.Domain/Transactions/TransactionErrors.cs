using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Transactions;

public static class TransactionErrors
{
    public static Error AccountNotFound(Guid accountId)
    {
        return Error.NotFound(
            "Transaction.Account.NotFound",
            $"The account with the Id = '{accountId}' was not found");
    }

    public static Error InsufficientFunds(Guid accountId)
    {
        return Error.Problem(
            "Transaction.Account.Insf",
            $"The account with the Id = '{accountId}' does not have sufficient funds");
    }
}