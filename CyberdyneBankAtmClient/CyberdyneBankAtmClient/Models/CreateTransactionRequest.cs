using CyberdyneBankAtmClient.Enums;

namespace CyberdyneBankAtmClient.Models
{
    public class CreateTransactionRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } // "Deposit" or "Withdraw"
        public string Description { get; set; }
    }
}
