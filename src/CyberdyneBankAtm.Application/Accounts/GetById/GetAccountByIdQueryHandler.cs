// GetAccountByIdQueryHandler.cs
using System.Data.Common;
using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CyberdyneBankAtm.Application.Accounts.GetById;

internal sealed class GetAccountByIdQueryHandler : IQueryHandler<GetAccountByIdQuery, AccountResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetAccountByIdQueryHandler> _logger;

    public GetAccountByIdQueryHandler(
        IApplicationDbContext context,
        ILogger<GetAccountByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<AccountResponse>> Handle(
        GetAccountByIdQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var account = await _context.Accounts
                .Where(a => a.Id == query.AccountId)
                .Select(a => new AccountResponse
                {
                    AccountId = a.Id,
                    Balance = a.Balance,
                    AccountType = a.AccountType,
                    CreatedOn = a.CreatedOn
                })
                .SingleOrDefaultAsync(cancellationToken);

            return account ?? Result.Failure<AccountResponse>(AccountErrors.NotFound(query.AccountId));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex,
                "Multiple accounts found with Id {AccountId}", query.AccountId);
            return Result.Failure<AccountResponse>(AccountErrors.MultipleFound(query.AccountId));
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex,
                "Retrieval of account {AccountId} was canceled", query.AccountId);
            return Result.Failure<AccountResponse>(AccountErrors.OperationCanceled());
        }
        catch (DbException ex)
        {
            _logger.LogError(ex,
                "Database error retrieving account {AccountId}", query.AccountId);
            return Result.Failure<AccountResponse>(AccountErrors.DatabaseError());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error retrieving account {AccountId}", query.AccountId);
            return Result.Failure<AccountResponse>(AccountErrors.UnknownError());
        }
    }
}
