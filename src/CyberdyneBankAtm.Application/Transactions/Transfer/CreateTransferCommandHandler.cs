using System.Data.Common;
using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Exceptions;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Transaction = CyberdyneBankAtm.Domain.Transactions.Transaction;

namespace CyberdyneBankAtm.Application.Transactions.Transfer;

internal sealed class CreateTransferCommandHandler : ICommandHandler<CreateTransferCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CreateTransferCommandHandler> _logger;

    public CreateTransferCommandHandler(
        IApplicationDbContext context,
        IDateTimeProvider dateTimeProvider,
        ILogger<CreateTransferCommandHandler> logger)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var accountFrom = await _context.Accounts
                .FindAsync(new object?[] { command.AccountId }, cancellationToken);

            if (accountFrom is null)
                return Result.Failure<int>(TransactionErrors.AccountNotFound(command.AccountId));

            var relatedAccountId = command.RelatedAccountId.GetValueOrDefault();
            var accountTo = await _context.Accounts
                .FindAsync(new object?[] { relatedAccountId }, cancellationToken);

            if (accountTo is null)
                return Result.Failure<int>(TransactionErrors.AccountNotFound(relatedAccountId));

            if (accountFrom.Balance < command.Amount)
                return Result.Failure<int>(TransactionErrors.InsufficientFunds(command.AccountId));

            // Prepare outgoing transaction
            var now = _dateTimeProvider.UtcNow;
            var transactionOut = new Transaction
            {
                AccountId = command.AccountId,
                RelatedAccountId = command.RelatedAccountId,
                Amount = command.Amount,
                TransactionType = TransactionType.TransferOut,
                Description = $"Transferring {command.Amount:C} out of account {command.AccountId} into account {relatedAccountId}",
                CreatedOn = now
            };
            accountFrom.Balance -= command.Amount;
            transactionOut.CurrentAccountBalance = accountFrom.Balance;
            transactionOut.Raise(new TransactionCompletedDomainEvent(
                transactionOut.Id,
                transactionOut.AccountId,
                transactionOut.CreatedOn,
                transactionOut.Amount,
                transactionOut.TransactionType,
                transactionOut.Description,
                transactionOut.RelatedAccountId,
                transactionOut.CurrentAccountBalance));
            _context.Transactions.Add(transactionOut);

            // Prepare incoming transaction
            var transactionIn = new Transaction
            {
                AccountId = relatedAccountId,
                RelatedAccountId = command.AccountId,
                Amount = command.Amount,
                TransactionType = TransactionType.TransferIn,
                Description = $"Transferring {command.Amount:C} into account {relatedAccountId} from account {command.AccountId}",
                CreatedOn = now
            };
            accountTo.Balance += command.Amount;
            transactionIn.CurrentAccountBalance = accountTo.Balance;
            transactionIn.Raise(new TransactionCompletedDomainEvent(
                transactionIn.Id,
                transactionIn.AccountId,
                transactionIn.CreatedOn,
                transactionIn.Amount,
                transactionIn.TransactionType,
                transactionIn.Description,
                transactionIn.RelatedAccountId,
                transactionIn.CurrentAccountBalance));
            _context.Transactions.Add(transactionIn);

            await _context.SaveChangesAsync(cancellationToken);

            return transactionOut.Id;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex,
                "Concurrency error transferring {Amount} from {From} to {To}",
                command.Amount, command.AccountId, command.RelatedAccountId);
            return Result.Failure<int>(TransactionErrors.ConcurrencyError());
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Database update error transferring {Amount} from {From} to {To}",
                command.Amount, command.AccountId, command.RelatedAccountId);
            return Result.Failure<int>(TransactionErrors.DatabaseError());
        }
        catch (DomainEventPublishException ex)
        {
            _logger.LogError(ex,
                "Domain event publish error during transfer from {From} to {To}",
                command.AccountId, command.RelatedAccountId);
            return Result.Failure<int>(TransactionErrors.EventPublishError());
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex,
                "Transfer operation canceled for transfer from {From} to {To}",
                command.AccountId, command.RelatedAccountId);
            return Result.Failure<int>(TransactionErrors.OperationCanceled());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error transferring {Amount} from {From} to {To}",
                command.Amount, command.AccountId, command.RelatedAccountId);
            return Result.Failure<int>(TransactionErrors.UnknownError());
        }
    }
}
