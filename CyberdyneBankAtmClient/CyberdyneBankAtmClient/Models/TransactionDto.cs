using CyberdyneBankAtmClient.Enums;

namespace CyberdyneBankAtmClient.Models
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal? CurrentAccountBalance { get; set; }
    }
}
