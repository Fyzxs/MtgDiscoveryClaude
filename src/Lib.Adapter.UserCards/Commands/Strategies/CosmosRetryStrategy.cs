using System;
using System.Net;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.UserCards.Commands.Strategies;

internal sealed class CosmosRetryStrategy : ICosmosRetryStrategy
{
    private readonly int _maxRetries;
    private readonly int _baseDelayMs;

    public CosmosRetryStrategy() : this(5, 50)
    { }

    private CosmosRetryStrategy(int maxRetries, int baseDelayMs)
    {
        _maxRetries = maxRetries;
        _baseDelayMs = baseDelayMs;
    }

    public async Task<IOperationResponse<T>> ExecuteWithRetry<T>(Func<Task<IOperationResponse<T>>> operation)
    {
        int retryCount = 0;

        while (retryCount < _maxRetries)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed && retryCount < _maxRetries)
            {
                // ETag mismatch detected - another request modified the document
                retryCount++;

                if (retryCount >= _maxRetries)
                {
                    return new FailureOperationResponse<T>(
                        new UserCardsAdapterException($"Failed to complete operation after {_maxRetries} retries due to concurrent updates", ex));
                }

                // Exponential backoff: 50ms, 100ms, 200ms, 400ms, 800ms
                int delayMs = _baseDelayMs * (1 << (retryCount - 1));
                await Task.Delay(delayMs).ConfigureAwait(false);

                // Loop will retry with fresh operation execution
            }
        }

        // Should never reach here due to loop logic, but satisfy compiler
        return new FailureOperationResponse<T>(
            new UserCardsAdapterException($"Failed to complete operation after {_maxRetries} retries"));
    }
}
