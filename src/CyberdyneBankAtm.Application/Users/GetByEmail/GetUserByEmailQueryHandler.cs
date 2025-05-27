using System.Data.Common;
using CyberdyneBankAtm.Application.Abstractions.Authentication;
using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Application.Abstractions.Messaging;
using CyberdyneBankAtm.Domain.Users;
using CyberdyneBankAtm.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CyberdyneBankAtm.Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetUserByEmailQueryHandler> _logger;

    public GetUserByEmailQueryHandler(
        IApplicationDbContext context,
        IUserContext userContext,
        ILogger<GetUserByEmailQueryHandler> logger)
    {
        _context = context;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<UserResponse>> Handle(
        GetUserByEmailQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users
                .Where(u => u.Email == query.Email)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);

            return user.Id != _userContext.UserId ? Result.Failure<UserResponse>(UserErrors.Unauthorized()) : user;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Multiple users found with email {Email}", query.Email);
            return Result.Failure<UserResponse>(UserErrors.MultipleUsersFound(query.Email));
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Database error while retrieving user by email {Email}", query.Email);
            return Result.Failure<UserResponse>(UserErrors.DatabaseError());
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation canceled while retrieving user by email {Email}", query.Email);
            return Result.Failure<UserResponse>(UserErrors.OperationCanceled());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving user by email {Email}", query.Email);
            return Result.Failure<UserResponse>(UserErrors.UnknownError());
        }
    }
}
