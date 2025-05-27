using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Transactions.Transfer;

public sealed class CreateTransferCommand : TransactionCommand, ICommand<int>
{
    public int? RelatedAccountId { get; set; }
}