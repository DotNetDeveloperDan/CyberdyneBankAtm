using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Exceptions;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Transaction = CyberdyneBankAtm.Domain.Transactions.Transaction;

namespace CyberdyneBankAtm.Application.Transactions.Create;

internal sealed class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CreateTransactionCommandHandler> _logger;

    public CreateTransactionCommandHandler(
        IApplicationDbContext context,
        IDateTimeProvider dateTimeProvider,
        ILogger<CreateTransactionCommandHandler> logger)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _context.Accounts
                .FindAsync(new object?[] { command.AccountId }, cancellationToken);

            if (account is null)
                return Result.Failure<int>(TransactionErrors.AccountNotFound(command.AccountId));

            var transaction = new Transaction
            {
                AccountId = command.AccountId,
                Amount = command.Amount,
                TransactionType = command.Type,
                Description = command.Description,
                CreatedOn = _dateTimeProvider.UtcNow
            };

            if (transaction.TransactionType == TransactionType.Deposit)
                account.Balance += transaction.Amount;
            else
                account.Balance -= transaction.Amount;

            transaction.CurrentAccountBalance = account.Balance;
            transaction.Raise(new TransactionCompletedDomainEvent(
                transaction.Id,
                transaction.AccountId,
                transaction.CreatedOn,
                transaction.Amount,
                transaction.TransactionType,
                transaction.Description,
                transaction.RelatedAccountId,
                transaction.CurrentAccountBalance));

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex,
                "Concurrency error creating transaction for account {AccountId}", command.AccountId);
            return Result.Failure<int>(TransactionErrors.ConcurrencyError());
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Database error creating transaction for account {AccountId}", command.AccountId);
            return Result.Failure<int>(TransactionErrors.DatabaseError());
        }
        catch (DomainEventPublishException ex)
        {
            _logger.LogError(ex,
                "Domain event publish error after creating transaction for account {AccountId}", command.AccountId);
            return Result.Failure<int>(TransactionErrors.EventPublishError());
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex,
                "Operation canceled while creating transaction for account {AccountId}", command.AccountId);
            return Result.Failure<int>(TransactionErrors.OperationCanceled());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error creating transaction for account {AccountId}", command.AccountId);
            return Result.Failure<int>(TransactionErrors.UnknownError());
        }
    }
}