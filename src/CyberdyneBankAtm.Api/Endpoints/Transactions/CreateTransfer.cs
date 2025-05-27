
using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Transactions.Transfer;
using CyberdyneBankAtm.Domain.Transactions;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Transactions
{
    public class CreateTransfer : IEndpoint
    {
        public sealed class Request
        {
            public int AccountId { get; set; } // The account this transaction is for
            public int? RelatedAccountId { get; set; } // The *other* account in a transfer (optional)
            public decimal Amount { get; set; }

        }
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("transactions/transfers", async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new CreateTransferCommand()
                    {
                        Amount = request.Amount,
                        AccountId = request.AccountId,
                        RelatedAccountId = request.RelatedAccountId
                    };

                    var result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                })
                .WithTags(Tags.Transactions)
                .WithOpenApi();
        }
    }
}
