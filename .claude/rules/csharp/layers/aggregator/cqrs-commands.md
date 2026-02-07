---
paths:
  - "csharp/src/Lib.Aggregators/**/Commands/*"
---

# Aggregator Command Behaviors

The concrete implementation of the `I{Type}CommandAggregator` interface defined in the APIs folder is here.
It uses constructor chaining to create instances of each behavior it exposes.

These are very targeted classes, following single responsibility by implementing a single behavior.

## Interface Pattern

The class and interfaces are named like `{Behavior}Aggregator` with interfaces having the `I` prefix.

```csharp
internal interface IAddUserWishlistCardAggregator
{
    Task<IOperationResponse<IUserWishlistCardOufEntity>> Execute(
        IUserWishlistCardItrEntity input,
        CancellationToken cancellationToken);
}
```

## Constructor Pattern

Public constructor takes `ILogger`, private constructor takes actual dependencies (adapter service + mappers):

```csharp
public AddUserCardAggregatorService(ILogger logger) : this(
    new UserCardsAdapterService(logger),
    new AddUserCardItrToXfrMapper(),
    new UserCardExtToOufEntityMapper())
{ }

private AddUserCardAggregatorService(
    IUserCardsAdapterService adapterService,
    IAddUserCardItrToXfrMapper itrToXfrMapper,
    IUserCardExtToOufEntityMapper extToOufMapper)
{
    _adapterService = adapterService;
    _itrToXfrMapper = itrToXfrMapper;
    _extToOufMapper = extToOufMapper;
}
```

## Execute Flow

All command aggregators follow this sequence:

1. Map `ItrEntity` → `XfrEntity` via injected mapper
2. Call adapter service with `XfrEntity` + `CancellationToken`
3. Check `response.IsFailure` — return `FailureOperationResponse` if failed
4. Map `ExtEntity` → `OufEntity` via injected mapper
5. Return `SuccessOperationResponse<IOufEntity>`

## Entity Type Conventions

- `TInput` must be an `IItrEntity` (Inflow internal entity)
- `TOutput` must be an `IOufEntity` (Outflow internal entity)

No exceptions.
