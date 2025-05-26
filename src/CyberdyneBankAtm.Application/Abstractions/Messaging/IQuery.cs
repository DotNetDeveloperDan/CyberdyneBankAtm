using CyberdyneBankAtm.SharedKernel;
using MediatR;

namespace CyberdyneBankAtm.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;