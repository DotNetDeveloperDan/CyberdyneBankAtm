using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Transactions;

public sealed record TransactionCompletedDomainEvent(
    Guid TransactionId,
    Guid AccountId,
    DateTime CreatedOn,
    decimal Amount,
    TransactionType TransactionType,
    string? Description = null,
    Guid? RelatedAccountId = null, // for transfers; null for deposit/withdraw
    Guid? TransferId = null // same for both TransferIn and TransferOut, enables grouping
) : IDomainEvent;