using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Transactions.Get;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Transactions;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("transactions", async (int accountId, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new GetTransactionsQuery(accountId);

                var result = await sender.Send(request, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Transactions)
            .WithOpenApi();
    }
}