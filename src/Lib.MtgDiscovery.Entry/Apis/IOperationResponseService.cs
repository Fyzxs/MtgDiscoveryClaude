using Lib.Shared.Abstractions.Services;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

/// <summary>
/// Base interface for Entry services that return IOperationResponse results.
/// Reduces type repetition and enables targeted abstractions for operation-based services.
/// </summary>
/// <typeparam name="TInput">The type of input parameter for the service operation.</typeparam>
/// <typeparam name="TOutput">The type of the successful response data (without IOperationResponse wrapper).</typeparam>
internal interface IOperationResponseService<in TInput, TOutput> : IServiceExecute<TInput, IOperationResponse<TOutput>>
{
}
