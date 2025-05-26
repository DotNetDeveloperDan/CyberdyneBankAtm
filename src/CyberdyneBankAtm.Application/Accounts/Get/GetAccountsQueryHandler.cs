using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Application.Accounts.Get;

public class GetAccountsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAccountsQuery, List<AccountResponse>>
{
    public async Task<Result<List<AccountResponse>>> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
    {
        var accounts = await context.Accounts
            .Where(account => account.UserId == query.UserId).Select(account => new AccountResponse
            {
                Balance = account.Balance,
                AccountType = account.AccountType,
                AccountId = account.Id,
                CreatedOn = account.CreatedOn
            }).ToListAsync(cancellationToken);

        return accounts;
    }
}