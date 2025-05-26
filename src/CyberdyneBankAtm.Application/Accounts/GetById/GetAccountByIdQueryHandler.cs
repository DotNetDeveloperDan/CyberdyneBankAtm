using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Application.Accounts.GetById;

public class GetAccountByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAccountByIdQuery, AccountResponse>
{
    public async Task<Result<AccountResponse>> Handle(GetAccountByIdQuery query, CancellationToken cancellationToken)
    {
        var account = await context.Accounts
            .Where(account => account.Id == query.AccountId).Select(account => new AccountResponse
            {
                Balance = account.Balance,
                AccountType = account.AccountType,
                AccountId = account.Id,
                CreatedOn = account.CreatedOn
            }).SingleOrDefaultAsync(cancellationToken);

        return account ?? Result.Failure<AccountResponse>(AccountErrors.NotFound(query.AccountId));
    }
}