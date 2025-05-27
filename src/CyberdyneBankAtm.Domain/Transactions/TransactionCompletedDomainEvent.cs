using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Transactions;

public sealed record TransactionCompletedDomainEvent(
    int TransactionId,
    int AccountId,
    DateTime CreatedOn,
    decimal Amount,
    TransactionType TransactionType,
    string? Description = null,
    int? RelatedAccountId = null, // for transfers; null for deposit/withdraw
    int? TransferId = null // same for both TransferIn and TransferOut, enables grouping
) : IDomainEvent;