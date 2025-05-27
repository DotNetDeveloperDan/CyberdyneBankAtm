using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Accounts.GetById;
using CyberdyneBankAtm.SharedKernel;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Accounts;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("accounts/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new GetAccountByIdQuery(id);

                var result = await sender.Send(request, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Accounts)
            .WithOpenApi();
    }
}