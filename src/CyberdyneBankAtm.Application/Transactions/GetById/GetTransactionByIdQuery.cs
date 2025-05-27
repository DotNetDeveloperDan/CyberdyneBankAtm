using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Transactions.GetById;

public sealed record GetTransactionByIdQuery(int TransactionId) : IQuery<TransactionResponse>;