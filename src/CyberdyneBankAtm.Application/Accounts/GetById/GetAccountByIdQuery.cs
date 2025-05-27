using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Accounts.GetById;

public sealed record GetAccountByIdQuery(int AccountId) : IQuery<AccountResponse>;