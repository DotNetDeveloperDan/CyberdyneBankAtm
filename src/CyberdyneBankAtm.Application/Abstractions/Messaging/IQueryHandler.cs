using CyberdyneBankAtm.SharedKernel;
using MediatR;

namespace CyberdyneBankAtm.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
