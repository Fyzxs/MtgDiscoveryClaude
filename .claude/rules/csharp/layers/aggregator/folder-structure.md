---
paths:
  - "csharp/src/Lib.Aggregators/Lib.Aggregator.*/**"
---

# Aggregator Folder Structure

## Canonical Layout

```
Lib.Aggregator.{Domain}/
├── Apis/                                    (public contract)
│   ├── I{Domain}AggregatorService.cs            (composite interface — inherits Command + Query)
│   ├── {Domain}AggregatorService.cs             (passthrough facade)
│   ├── I{Domain}CommandAggregatorService.cs     (command interface)
│   └── I{Domain}QueryAggregatorService.cs       (query interface)
├── Commands/                                (command implementations)
│   ├── Entities/                                (XfrEntity files for commands ONLY)
│   ├── Mappers/                                 (ItrToXfr + ExtToOuf mappers for commands)
│   ├── {Domain}CommandAggregator.cs             (router — implements command interface)
│   ├── I{Behavior}Aggregator.cs                 (internal behavior interface)
│   └── {Behavior}Aggregator.cs                  (internal behavior implementation)
├── Entities/                                (OufEntity files — ALL OufEntities live here)
├── Exceptions/                              (aggregator-specific exceptions)
├── Mappers/                                 (shared mappers — ONLY when used by both commands AND queries)
├── Queries/                                 (query implementations)
│   ├── Entities/                                (XfrEntity files for queries ONLY)
│   ├── Mappers/                                 (ItrToXfr + ExtToOuf mappers for queries)
│   ├── {Domain}QueryAggregator.cs               (router — implements query interface)
│   ├── I{Behavior}Aggregator.cs                 (internal behavior interface)
│   └── {Behavior}Aggregator.cs                  (internal behavior implementation)
```

## Key Rules

### Naming

- **Public scope** (`Apis/`): Uses `AggregatorService` suffix — e.g., `CollectionsAggregatorService`
- **Internal scope** (`Commands/`, `Queries/`): Uses `Aggregator` suffix — e.g., `AddUserCardAggregator`

See: `apis.md` for the scope rule explanation.

### Router Classes

Each CQRS side has a **router class** that implements the CQRS interface from `Apis/` and delegates to individual behavior classes:

- `{Domain}CommandAggregator.cs` in `Commands/` — implements `I{Domain}CommandAggregatorService`
- `{Domain}QueryAggregator.cs` in `Queries/` — implements `I{Domain}QueryAggregatorService`

See: `cqrs-commands.md` and `cqrs-queries.md` for router class details.

### OufEntity Location

ALL OufEntities MUST live at the project root `Entities/` folder. OufEntities MUST NOT be placed inside `Queries/Entities/` or `Commands/Entities/`.

The `Queries/Entities/` and `Commands/Entities/` folders hold **XfrEntities only**.

### Mapper Location

- **`Queries/Mappers/`** — ItrToXfr mappers AND ExtToOuf mappers used by query behaviors
- **`Commands/Mappers/`** — ItrToXfr mappers AND ExtToOuf mappers used by command behaviors
- **Root `Mappers/`** — ONLY for mappers shared across both commands AND queries

Most ExtToOuf mappers are specific to either commands or queries and belong in the respective subfolder. Root `Mappers/` is uncommon.

### Exceptions

Exception classes live in `Exceptions/` at the project root. Not all projects need exceptions — only add them when aggregator-specific error context is required.

- Naming: `{Domain}AggregatorOperationException` or `{Domain}AggregatorException`
- Extends `OperationException` with `HttpStatusCode.InternalServerError`
- Requires `#pragma warning disable CA1032`

```csharp
#pragma warning disable CA1032
internal sealed class CardAggregatorOperationException : OperationException
#pragma warning restore CA1032
{
    public CardAggregatorOperationException(string message, Exception innerException = null)
        : base(HttpStatusCode.InternalServerError, message, innerException)
    { }
}
```

### Structure Flexibility

- **Query-only projects**: May omit `Commands/` entirely (e.g., `Lib.Aggregator.Cards`).
- **Command-only projects**: May omit `Queries/` entirely.
- **Sub-feature folders are NOT canonical**: Behaviors should be flat within `Commands/` or `Queries/`. Do not nest behaviors in subfolders like `Queries/UserCardsForSigning/`.
- **`Models/` is not canonical**: Some projects use `Models/` but it is not part of the canonical structure.

## Reference Implementation

`Lib.Aggregator.Collections/` follows this structure exactly and is the canonical reference.
