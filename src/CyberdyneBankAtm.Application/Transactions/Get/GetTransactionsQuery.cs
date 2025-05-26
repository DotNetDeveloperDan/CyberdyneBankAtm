using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Transactions.Get;

public sealed record GetTransactionsQuery(Guid AccountId) : IQuery<List<TransactionResponse>>;