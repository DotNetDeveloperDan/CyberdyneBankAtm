namespace CyberdyneBankAtmClient.Models
{
    public class CreateTransferRequest
    {
        public int AccountId { get; set; }
        public int RelatedAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
