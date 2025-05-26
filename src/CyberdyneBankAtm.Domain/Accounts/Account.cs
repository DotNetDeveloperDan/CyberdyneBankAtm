using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.Domain.Users;
using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Domain.Accounts;

public class Account : Entity
{
    public Guid UserId { get; set; }
    public List<Transaction> Transactions { get; private set; } = [];
    public AccountType AccountType { get; private set; }
    public decimal Balance { get; set; }
    public DateTime CreatedOn { get; set; }
    public User User { get; set; }
}