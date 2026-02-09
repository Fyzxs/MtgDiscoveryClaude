---
paths:
  - "csharp/src/**/Validators/**"
---

# Validator Pattern

## Purpose

Validators **check input correctness before processing**, returning structured failure responses when validation fails. They compose into containers that fail-fast on the first invalid rule.

## Base Types

All validators derive from types in `Lib.Shared.Abstractions.Actions.Validators`:

| Type | Purpose |
|------|---------|
| `IValidator<TItem>` | Boolean validation logic |
| `IValidatorAction<TItem, TFailureStatus>` | Validates and returns structured result |
| `IValidatorActionResult<TFailureStatus>` | Result container (valid or failure status) |
| `ValidatorActionContainer<TItem, TFailureStatus>` | Composes multiple validators, fails-fast |
| `OperationResponseValidator<TValidationType, TReturnType>` | Bridges `IValidator` to `IOperationResponse` failure |

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Container interface | `I{Operation}ArgEntityValidator` | `IAddCardToCollectionArgEntityValidator` |
| Container implementation | `{Operation}ArgEntityValidatorContainer` | `AddCardToCollectionArgEntityValidatorContainer` |
| Individual validator | `{Rule}{Operation}Validator` or `HasValid{Property}{Operation}Validator` | `HasValidCardIdAddCardToCollectionArgEntityValidator` |

## Validator Container

The container composes multiple validators and fails-fast on the first invalid result:

**See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Actions/Validators/AddCardToCollectionArgEntityValidatorContainer.cs`

**Key points:**
- Container class extends `ValidatorActionContainer<TItem, TFailureStatus>` AND implements the container interface
- Constructor passes validator array to base — validators run in order, fail-fast
- No logic in the container — pure composition

## Individual Validator

Each validator is a self-contained class with nested `Validator` and `Message` classes:

**See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Actions/Validators/HasValidCardIdAddCardToCollectionArgEntityValidator.cs`

**Key points:**
- Extends `OperationResponseValidator<TValidationType, TReturnType>`
- Constructor passes `new Validator()` and `new Message()` to base
- Inner `Validator` class implements `IValidator<T>` with boolean logic
- Inner `Message` class extends `OperationResponseMessage` with error text
- All validation logic is synchronous, wrapped in `Task.FromResult()`

## Validator with Static Validation Data

For validators that check against a fixed set of valid values:

**See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Actions/Validators/CollectedItemFinishValidator.cs`

**Key points:**
- Static readonly collections for valid value sets are acceptable
- Field naming: `s_` prefix for static fields
- Guard against null before accessing nested properties

## Async Validators

For validators that need to call external services (e.g., uniqueness checks against a domain service):

**See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Collections/Validators/Uniqueness/ICreateCollectionNameUniquenessValidator.cs`

**Key differences from standard validators:**
- Accept `CancellationToken` — they perform async operations (domain service calls, database lookups)
- Define a custom interface — they do NOT extend `OperationResponseValidator<T, R>` or participate in the `ValidatorActionContainer` pipeline
- Called individually in the service method, separate from the validator container's fail-fast chain
- Injected as a dependency into the operation service (not composed into the container array)

**Location:** `Commands/{Domain}/Validators/Uniqueness/` or alongside the operation that uses them.

**Usage in service:**

**See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Collections/CollectionEntryCommandService.cs`

## Location

| Type | Location |
|------|----------|
| Command validators (shared) | `Commands/Actions/Validators/` |
| Command validators (domain) | `Commands/Actions/Validators/{Domain}/` or `Commands/{Domain}/Validators/` |
| Query validators (shared) | `Queries/Actions/Validators/` |
| Query validators (domain) | `Queries/Actions/Validators/{Domain}/` |

## Usage in Entry Services

**See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry/Commands/Collections/CollectionEntryCommandService.cs`

Validation always happens first, before any mapping or domain calls.

## Reference Implementations

- **Container**: `Commands/Actions/Validators/AddCardToCollectionArgEntityValidatorContainer.cs`
- **Individual validator**: `Commands/Actions/Validators/HasValidCardIdAddCardToCollectionArgEntityValidator.cs`
- **Static data validator**: `Commands/Actions/Validators/CollectedItemFinishValidator.cs`
- **Collection validators**: `Commands/Collections/Validators/`
