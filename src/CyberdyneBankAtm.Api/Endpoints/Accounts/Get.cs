using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Accounts.Get;
using CyberdyneBankAtm.SharedKernel;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Accounts;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("accounts", async (int userId, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new GetAccountsQuery(userId);

                var result = await sender.Send(request, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Accounts)
            .WithOpenApi();

    }
}