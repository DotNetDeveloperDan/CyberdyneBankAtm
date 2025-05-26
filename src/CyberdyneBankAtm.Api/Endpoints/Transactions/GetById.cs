using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Transactions.GetById;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Transactions;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("transactions/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new GetTransactionByIdQuery(id);

                var result = await sender.Send(request, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Transactions);
    }
}