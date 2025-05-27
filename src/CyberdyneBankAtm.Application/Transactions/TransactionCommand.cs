namespace CyberdyneBankAtm.Application.Transactions;

public abstract class TransactionCommand
{
    public int AccountId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedOn { get; set; }
}