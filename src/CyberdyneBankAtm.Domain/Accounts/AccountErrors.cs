using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Accounts;

public static class AccountErrors
{
    public static Error NotFound(Guid accountId)
    {
        return Error.NotFound(
            "Accounts.NotFound",
            $"The account with the Id = '{accountId}' was not found");
    }
}