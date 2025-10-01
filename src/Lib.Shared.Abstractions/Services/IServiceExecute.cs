using System.Threading.Tasks;

namespace Lib.Shared.Abstractions.Services;

/// <summary>
/// Base interface for all service execution patterns.
/// Provides a consistent contract for service operations with single input and output.
/// </summary>
/// <typeparam name="TInput">The type of input parameter for the service operation.</typeparam>
/// <typeparam name="TOutput">The type of output result from the service operation.</typeparam>
public interface IServiceExecute<in TInput, TOutput>
{
    /// <summary>
    /// Executes the service operation with the provided input.
    /// </summary>
    /// <param name="input">The input parameter for the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result.</returns>
    Task<TOutput> Execute(TInput input);
}
