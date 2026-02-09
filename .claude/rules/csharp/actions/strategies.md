---
paths:
  - "csharp/src/**/Strategies/**"
---

# Strategy Pattern

## Purpose

Strategies **wrap operations with cross-cutting concerns** such as retry logic, circuit breakers, or rate limiting. They handle transient failures and optimistic concurrency conflicts without polluting the main operation logic.

## Base Interface

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/Strategies/ICosmosRetryStrategy.cs`

**Location**: `Lib.Adapter.UserCards/Commands/Strategies/ICosmosRetryStrategy.cs`

## Naming Convention

`{Concern}Strategy` — e.g., `CosmosRetryStrategy`, `RateLimitStrategy`

## Implementation Pattern

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/Strategies/CosmosRetryStrategy.cs`

**Key points:**
- Constructor chain pattern for defaults
- Exponential backoff for retry delays
- Catches specific exceptions (PreconditionFailed for ETag conflicts)
- Returns `FailureOperationResponse` on exhaustion, never throws

## Usage in Command Adapters

> **See:** `csharp/src/Lib.Adapters/Lib.Adapter.UserCards/Commands/AddUserCardAdapter.cs`

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
