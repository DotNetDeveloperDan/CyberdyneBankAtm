using CyberdyneBankAtmClient.Enums;

namespace CyberdyneBankAtmClient.Models
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
