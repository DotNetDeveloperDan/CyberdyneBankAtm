using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Transactions.GetById;

public sealed record GetTransactionByIdQuery(Guid TransactionId) : IQuery<TransactionResponse>;