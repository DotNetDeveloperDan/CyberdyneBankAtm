using System.Data.Common;
using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CyberdyneBankAtm.Application.Accounts.Get;

internal sealed class GetAccountsQueryHandler(
    IApplicationDbContext context,
    ILogger<GetAccountsQueryHandler> logger)
    : IQueryHandler<GetAccountsQuery, List<AccountResponse>>
{
    public async Task<Result<List<AccountResponse>>> Handle(
        GetAccountsQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var accounts = await context.Accounts
                .Where(account => account.UserId == query.UserId)
                .Select(account => new AccountResponse
                {
                    AccountId = account.Id,
                    Balance = account.Balance,
                    AccountType = account.AccountType,
                    CreatedOn = account.CreatedOn
                })
                .ToListAsync(cancellationToken);

            return accounts;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex,
                "Retrieving accounts for user {UserId} was canceled.",
                query.UserId);
            return Result.Failure<List<AccountResponse>>(AccountErrors.OperationCanceled());
        }
        catch (DbException ex)
        {
            logger.LogError(ex,
                "Database error while retrieving accounts for user {UserId}.",
                query.UserId);
            return Result.Failure<List<AccountResponse>>(AccountErrors.DatabaseError());
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while retrieving accounts for user {UserId}.",
                query.UserId);
            return Result.Failure<List<AccountResponse>>(AccountErrors.UnknownError());
        }
    }
}
