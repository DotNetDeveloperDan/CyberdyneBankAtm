using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Users;
using CyberdyneBankAtm.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher<> passwordHasher,
    ITokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is null) return Result.Failure<string>(UserErrors.NotFoundByEmail);

        bool verified = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!verified) return Result.Failure<string>(UserErrors.NotFoundByEmail);

        string token = tokenProvider.Create(user);

        return token;
    }
}