using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Application.Transactions.GetById;

public class GetTransactionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
{
    /// <summary>Handles the specified query.</summary>
    /// <param name="query">The query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <br />
    /// </returns>
    public async Task<Result<TransactionResponse>> Handle(GetTransactionByIdQuery query,
        CancellationToken cancellationToken)
    {
        var transaction = await context.Transactions
            .Where(transaction => transaction.Id == query.TransactionId).Select(transaction => new TransactionResponse
            {
                Amount = transaction.Amount,
                Description = transaction.Description,
                TransactionId = transaction.Id,
                TransactionType = transaction.TransactionType,
                CreatedOn = transaction.CreatedOn
            }).SingleOrDefaultAsync(cancellationToken);

        return transaction ?? Result.Failure<TransactionResponse>(TransactionErrors.NotFound(query.TransactionId));
    }
}