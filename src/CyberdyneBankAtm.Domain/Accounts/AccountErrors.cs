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
}