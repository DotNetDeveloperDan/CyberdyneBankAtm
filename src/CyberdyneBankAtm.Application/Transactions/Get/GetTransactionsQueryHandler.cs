using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Application.Transactions.Get;

public class GetTransactionsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTransactionsQuery, List<TransactionResponse>>
{
    public async Task<Result<List<TransactionResponse>>> Handle(GetTransactionsQuery query,
        CancellationToken cancellationToken)
    {
        var transactions = await context.Transactions
            .Where(transaction => transaction.AccountId == query.AccountId).Select(transaction =>
                new TransactionResponse
                {
                    Amount = transaction.Amount,
                    TransactionId = transaction.Id,
                    TransactionType = transaction.TransactionType,
                    Description = transaction.Description,
                    CreatedOn = transaction.CreatedOn
                }).ToListAsync(cancellationToken);

        return transactions;
    }
}