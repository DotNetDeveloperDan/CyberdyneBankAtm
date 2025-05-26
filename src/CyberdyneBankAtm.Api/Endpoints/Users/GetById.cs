using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Users.GetById;
using CyberdyneBankAtm.SharedKernel;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{userId}", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(userId);

            Result<UserResponse> result = await sender.Send(query, cancellationToken) as Result<UserResponse>;

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
