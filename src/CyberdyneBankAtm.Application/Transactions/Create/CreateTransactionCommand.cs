using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Transactions;

namespace CyberdyneBankAtm.Application.Transactions.Create;

public sealed class CreateTransactionCommand :TransactionCommand, ICommand<Guid>
{
    public TransactionType Type { get; set; }
}