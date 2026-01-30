using System;
using System.Threading.Tasks;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Commands.Strategies;

/// <summary>
/// Defines a retry strategy for Cosmos DB operations that may encounter optimistic concurrency conflicts.
/// </summary>
internal interface ICosmosRetryStrategy
{
    /// <summary>
    /// Executes the provided operation with retry logic for handling ETag conflicts.
    /// </summary>
    /// <typeparam name="T">The result type of the operation.</typeparam>
    /// <param name="operation">The operation to execute with retry support.</param>
    /// <returns>The operation response after successful execution or maximum retries.</returns>
    Task<IOperationResponse<T>> ExecuteWithRetry<T>(Func<Task<IOperationResponse<T>>> operation);
}
