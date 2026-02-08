---
paths:
  - "csharp/src/Lib.Domains/Lib.Domain.*/**"
---

# Domain Folder Structure

## Canonical Layout

```
Lib.Domain.{Domain}/
├── Apis/                                        (public contract)
│   ├── I{Domain}DomainService.cs                    (composite interface — inherits Command + Query)
│   ├── {Domain}DomainService.cs                     (passthrough facade)
│   ├── I{Domain}CommandDomainService.cs             (command interface)
│   └── I{Domain}QueryDomainService.cs               (query interface)
├── Commands/                                    (command implementations)
│   ├── {Domain}CommandDomainService.cs              (router — implements command interface)
│   ├── I{Behavior}DomainService.cs                  (specialized operation interface)
│   └── {Behavior}DomainService.cs                   (specialized operation implementation)
└── Queries/                                     (query implementations)
    ├── {Domain}QueryDomainService.cs                (router — implements query interface)
    ├── I{Behavior}DomainService.cs                  (specialized operation interface)
    └── {Behavior}DomainService.cs                   (specialized operation implementation)
```

## Key Rules

### Naming

All domain classes use the `DomainService` suffix — there is no scope-based suffix distinction (unlike the aggregator layer).

- Facade: `{Domain}DomainService` (public)
- Router: `{Domain}CommandDomainService` / `{Domain}QueryDomainService` (internal)
- Specialized: `{Behavior}DomainService` (internal)

The router naming pattern is `{Domain}{Command|Query}DomainService` — domain name first, then CQRS type.

### Router Classes

Each CQRS side has a **router class** that implements the CQRS interface from `Apis/`:

- `{Domain}CommandDomainService.cs` in `Commands/` — implements `I{Domain}CommandDomainService`
- `{Domain}QueryDomainService.cs` in `Queries/` — implements `I{Domain}QueryDomainService`

Routers use one of two delegation patterns. See `cqrs-commands.md` and `cqrs-queries.md` for details.

### No Entities, Mappers, or Exceptions

The domain layer performs **zero transformation**. It owns no entity types, no mappers, and no exception classes.

- `Entities/` — does NOT exist in domain projects
- `Mappers/` — does NOT exist in domain projects
- `Exceptions/` — does NOT exist in domain projects

ItrEntities come from the Entry/Shared layer. OufEntities come from the Aggregator layer. The domain passes them through unchanged.

### Structure Flexibility

- **Query-only projects**: May omit `Commands/` entirely (e.g., `Lib.Domain.Artists`, `Lib.Domain.Cards`).
- **Command-only projects**: May omit `Queries/` entirely (e.g., `Lib.Domain.User`).
- **Specialized operations are optional**: When a router only delegates directly to the aggregator (no `Execute()` pattern), no specialized service files are needed.

## Reference Implementation

`Lib.Domain.Collections/` demonstrates the direct passthrough pattern.
`Lib.Domain.Artists/` demonstrates the specialized operations pattern.
