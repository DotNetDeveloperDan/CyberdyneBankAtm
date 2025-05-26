using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Accounts.GetById;

public sealed record GetAccountByIdQuery(Guid AccountId) : IQuery<AccountResponse>;