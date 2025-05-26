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
        app.MapGet("accounts", async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetAccountsQuery(userId);

                Result<List<AccountResponse>> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Accounts);

    }
}