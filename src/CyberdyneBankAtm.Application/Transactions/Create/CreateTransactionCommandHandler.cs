using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Application.Transactions.Create
{
    internal sealed class CreateTransactionCommandHandler(
        IApplicationDbContext context,
        IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateTransactionCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            var account = await context.Accounts.FindAsync([command.AccountId], cancellationToken);

            if (account == null)
            {
                return Result.Failure<Guid>(TransactionErrors.AccountNotFound(command.AccountId));
            }

            var transaction = new Transaction()
            {
                AccountId = command.AccountId,
                Amount = command.Amount,
                TransactionType = command.Type,
                Description = command.Description,
                CreatedOn = dateTimeProvider.UtcNow
            };

            transaction.Raise(new TransactionCompletedDomainEvent(transaction.Id,transaction.AccountId,transaction.CreatedOn,transaction.Amount, transaction.TransactionType,transaction.Description, transaction.RelatedAccountId));
            context.Transactions.Add(transaction);
            if (transaction.TransactionType == TransactionType.Deposit)
            {
                account.Balance += transaction.Amount;
            }
            else
            {
                account.Balance -= transaction.Amount;
            }
                await context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}
