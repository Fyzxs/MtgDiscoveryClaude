---
paths:
  - "csharp/src/**/Strategies/**"
---

# Strategy Pattern

## Purpose

Strategies **wrap operations with cross-cutting concerns** such as retry logic, circuit breakers, or rate limiting. They handle transient failures and optimistic concurrency conflicts without polluting the main operation logic.

## Base Interface

```csharp
internal interface ICosmosRetryStrategy
{
    Task<IOperationResponse<T>> ExecuteWithRetry<T>(Func<Task<IOperationResponse<T>>> operation);
}
```

**Location**: `Lib.Adapter.UserCards/Commands/Strategies/ICosmosRetryStrategy.cs`

## Naming Convention

`{Concern}Strategy` — e.g., `CosmosRetryStrategy`, `RateLimitStrategy`

## Implementation Pattern

```csharp
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

    public async Task<IOperationResponse<T>> ExecuteWithRetry<T>(
        Func<Task<IOperationResponse<T>>> operation)
    {
        int retryCount = 0;

        while (retryCount < _maxRetries)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                retryCount++;
                if (retryCount >= _maxRetries)
                {
                    return new FailureOperationResponse<T>(
                        new AdapterException($"Failed after {_maxRetries} retries", ex));
                }

                // Exponential backoff: 50ms, 100ms, 200ms, 400ms, 800ms
                int delayMs = _baseDelayMs * (1 << (retryCount - 1));
                await Task.Delay(delayMs).ConfigureAwait(false);
            }
        }

        return new FailureOperationResponse<T>(new AdapterException("Max retries exceeded"));
    }
}
```

**Key points:**
- Constructor chain pattern for defaults
- Exponential backoff for retry delays
- Catches specific exceptions (PreconditionFailed for ETag conflicts)
- Returns `FailureOperationResponse` on exhaustion, never throws

## Usage in Command Adapters

```csharp
internal sealed class AddUserCardAdapter : IAddUserCardAdapter
{
    private readonly ICosmosRetryStrategy _retryStrategy;
    // ... other dependencies

    public AddUserCardAdapter(ILogger logger)
        : this(..., new CosmosRetryStrategy())
    { }

    public async Task<IOperationResponse<UserCardExtEntity>> Execute(
        IAddUserCardXfrEntity input,
        CancellationToken cancellationToken)
    {
        return await _retryStrategy.ExecuteWithRetry<UserCardExtEntity>(async () =>
        {
            // Read-modify-write cycle
            ReadPointItem readPoint = await _readPointMapper.Map(input);
            OpResponse<UserCardExtEntity> readResponse = await _gopher.ReadAsync<UserCardExtEntity>(readPoint);
            UserCardExtEntity existing = _resolver.Resolve(readResponse, input);
            UserCardExtEntity updated = await _integrator.Integrate(existing, input);
            OpResponse<UserCardExtEntity> upsertResponse = await _scribe.UpsertAsync(updated);

            return upsertResponse.IsSuccessful()
                ? new SuccessOperationResponse<UserCardExtEntity>(upsertResponse.Value)
                : new FailureOperationResponse<UserCardExtEntity>(...);
        }).ConfigureAwait(false);
    }
}
```

## When to Use Strategies

**Use strategies for:**
- **Optimistic concurrency** — ETag conflicts in Cosmos read-modify-write patterns
- **Transient failures** — Network timeouts, throttling (429 errors)
- **Rate limiting** — External API call budgeting

**Don't use strategies for:**
- **Business logic errors** — Invalid input, authorization failures
- **Permanent failures** — Resource not found (404), bad request (400)

## Location in Adapters

`{Adapter}/Commands/Strategies/`

Currently implemented in:
- `Lib.Adapter.UserCards/Commands/Strategies/` — `CosmosRetryStrategy`

## Retry Configuration

| Parameter | Default | Purpose |
|-----------|---------|---------|
| `maxRetries` | 5 | Maximum retry attempts |
| `baseDelayMs` | 50 | Base delay for exponential backoff |

**Backoff schedule**: 50ms → 100ms → 200ms → 400ms → 800ms

## Related Patterns

- **Gopher**: Source of point-read operations that may fail — see `../cosmos/cosmos-gopher.md`
- **Scribe**: Target of upsert operations that may conflict — see `../cosmos/cosmos-scribe.md`
- **Integrator**: Merge logic executed within retry loop — see `integrators.md`
