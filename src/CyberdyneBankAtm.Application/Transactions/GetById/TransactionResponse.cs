using CyberdyneBankAtm.Domain.Transactions;

namespace CyberdyneBankAtm.Application.Transactions.GetById;

public class TransactionResponse
{
    public int TransactionId { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; }
}