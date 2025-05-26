using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Transaction> Transactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
