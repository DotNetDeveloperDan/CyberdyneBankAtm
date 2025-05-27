using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Transactions.Get;

public sealed record GetTransactionsQuery(int AccountId) : IQuery<List<TransactionResponse>>;