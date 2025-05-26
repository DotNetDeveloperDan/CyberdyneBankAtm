using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Transactions.Transfer;

public sealed class CreateTransferCommand : TransactionCommand, ICommand<Guid>
{
    public Guid? RelatedAccountId { get; set; }
}