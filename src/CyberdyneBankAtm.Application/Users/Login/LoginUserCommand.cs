using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;