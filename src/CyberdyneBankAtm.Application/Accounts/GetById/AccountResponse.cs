using CyberdyneBankAtm.Domain.Accounts;

namespace CyberdyneBankAtm.Application.Accounts.GetById;

public class AccountResponse
{
    public Guid AccountId { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedOn { get; set; }
}