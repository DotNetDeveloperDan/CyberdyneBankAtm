using CyberdyneBankAtm.Api.Extensions;
using CyberdyneBankAtm.Api.Infrastructure;
using CyberdyneBankAtm.Application.Users.Login;
using CyberdyneBankAtm.SharedKernel;
using MediatR;

namespace CyberdyneBankAtm.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);

            Result<string> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
