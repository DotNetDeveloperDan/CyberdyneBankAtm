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
                    Type = request.TransactionType
                };

                var result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Transactions);
    }

    public sealed class Request
    {
        public Guid AccountId { get; set; } // The account this transaction is for
        public TransactionType TransactionType { get; set; } // "Deposit", "Withdrawal"
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}