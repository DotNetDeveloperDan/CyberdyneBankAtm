using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Transactions.Create;
using CyberdyneBankAtm.Domain.Transactions;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Transactions;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("transactions", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new CreateTransactionCommand
                {
                    Amount = request.Amount,
                    Description = request.Description,
                    AccountId = request.AccountId,
                    Type = request.Type
                };

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .WithTags(Tags.Transactions)
            .WithOpenApi();
    }

    public sealed class Request
    {
        public int AccountId { get; set; } // The account this transaction is for
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } // "Deposit", "Withdrawal"
        public string Description { get; set; }

    }
}