using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Transactions;

namespace CyberdyneBankAtm.Application.Transactions.Create;

public sealed class CreateTransactionCommand : TransactionCommand, ICommand<int>
{
    public TransactionType Type { get; set; }
}