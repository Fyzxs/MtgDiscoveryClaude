using System.Threading.Tasks;
using Lib.Shared.Abstractions.Services;
using Lib.Shared.Invocation.Operations;

namespace Lib.Shared.Invocation.Services;

/// <summary>
/// Base interface for services that return IOperationResponse.
/// Wraps IServiceExecute with operation response pattern.
/// </summary>
/// <typeparam name="TInput">The type of input parameter for the service operation.</typeparam>
/// <typeparam name="TOutput">The type of output data wrapped in IOperationResponse.</typeparam>
public interface IOperationResponseService<in TInput, TOutput> : IServiceExecute<TInput, IOperationResponse<TOutput>>
{
}
