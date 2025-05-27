using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Accounts.Get;

public sealed record GetAccountsQuery(int UserId) : IQuery<List<AccountResponse>>;