using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.SharedKernel;
using Transaction = CyberdyneBankAtm.Domain.Transactions.Transaction;

namespace CyberdyneBankAtm.Application.Transactions.Transfer;

internal sealed class CreateTransferCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<CreateTransferCommand, int>
{
    public async Task<Result<int>> Handle(CreateTransferCommand command, CancellationToken cancellationToken)
    {
        var accountFrom = await context.Accounts.FindAsync([command.AccountId], cancellationToken);

        if (accountFrom == null) return Result.Failure<int>(TransactionErrors.AccountNotFound(command.AccountId));

        var accountTo = await context.Accounts.FindAsync([command.RelatedAccountId], cancellationToken);

        if (accountTo == null)
            return Result.Failure<int>(
                TransactionErrors.AccountNotFound(command.RelatedAccountId.GetValueOrDefault()));

        if (accountFrom.Balance < command.Amount)
            return Result.Failure<int>(TransactionErrors.InsufficientFunds(command.AccountId));

        var transactionOut = new Transaction
        {
            AccountId = command.AccountId,
            RelatedAccountId = command.RelatedAccountId,
            Amount = command.Amount,
            TransactionType = TransactionType.TransferOut,
            Description =
                $"Transferring {command.Amount} out of account: {command.AccountId} into account: {command.RelatedAccountId.GetValueOrDefault()}",
            CreatedOn = dateTimeProvider.UtcNow
        };

        var transactionIn = new Transaction
        {
            AccountId = command.RelatedAccountId.GetValueOrDefault(),
            RelatedAccountId = command.AccountId,
            Amount = command.Amount,
            TransactionType = TransactionType.TransferIn,
            Description =
                $"Transferring {command.Amount} into account: {command.RelatedAccountId.GetValueOrDefault()} from account: {command.AccountId}",
            CreatedOn = dateTimeProvider.UtcNow
        };
        context.Transactions.Add(transactionOut);
        accountFrom.Balance -= command.Amount;
        context.Transactions.Add(transactionIn);
        accountTo.Balance += command.Amount;

        await context.SaveChangesAsync(cancellationToken);

        return transactionOut.Id;
    }
}