using CyberdyneBankAtm.Application.Abstractions.Messaging;

namespace CyberdyneBankAtm.Application.Users.GetById;

public sealed record GetUserByIdQuery(int UserId) : IQuery<UserResponse>;