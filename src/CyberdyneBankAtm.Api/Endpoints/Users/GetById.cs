using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Users.GetById;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{id:int}", async (int id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetUserByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users)
            .WithOpenApi();
    }
}