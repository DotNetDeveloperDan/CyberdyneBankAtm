using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Transactions;

public class Transaction : Entity
{
    public Guid AccountId { get; set; } // The account this transaction is for
    public Guid? RelatedAccountId { get; set; } // The *other* account in a transfer (optional)
    public TransactionType TransactionType { get; set; } // "Deposit", "Withdrawal"
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; }
}