---
paths:
  - "csharp/src/Lib.Aggregators/Lib.Aggregator.*/**"
---

# Aggregator Folder Structure

## Canonical Layout

```
Lib.Aggregator.{Domain}/
├── Apis/                              (public contract)
│   ├── I{Domain}AggregatorService.cs       (composite interface — inherits Command + Query)
│   ├── {Domain}AggregatorService.cs        (passthrough facade)
│   ├── I{Domain}CommandAggregatorService.cs (command interface)
│   └── I{Domain}QueryAggregatorService.cs   (query interface)
├── Commands/                          (command aggregator services)
│   ├── Entities/                          (XfrEntity files for commands)
│   ├── Mappers/                           (ItrToXfr mappers for commands)
│   ├── I{Behavior}Aggregator.cs           (internal operation interface)
│   └── {Behavior}Aggregator.cs            (internal operation implementation)
├── Entities/                          (OufEntity files — shared output)
├── Exceptions/                        (aggregator-specific exceptions)
├── Mappers/                           (ExtToOuf mappers — shared response)
├── Queries/                           (query aggregator services)
│   ├── Entities/                          (XfrEntity files for queries)
│   ├── Mappers/                           (ItrToXfr + ExtToOuf mappers for queries)
│   ├── {Operation}{Domain}AggregatorService.cs
│   └── I{Operation}{Domain}AggregatorService.cs
```

## Key Rules

- **Apis/ is public only**: Everything in `Apis/` must be `public` scoped. Internal operation classes do NOT go here.
- **Operations go in Commands/ or Queries/**: Specialized aggregator services live directly in `Commands/` or `Queries/`, not nested under `Apis/`.
- **Shared OufEntities at root**: `Entities/` at project root holds OufEntity types shared across commands and queries.
- **Shared ExtToOuf mappers**: `Mappers/` at project root holds response mappers used by both commands and queries.
- **Query-only projects**: May omit `Commands/` entirely (e.g., `Lib.Aggregator.Cards`).
- **Command-only projects**: May omit `Queries/` entirely.

## Reference Implementation

`Lib.Aggregator.Collections/` follows this structure exactly and is the canonical reference.
