using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Accounts.Get;

public sealed record GetAccountsQuery(Guid UserId) : IQuery<List<AccountResponse>>;