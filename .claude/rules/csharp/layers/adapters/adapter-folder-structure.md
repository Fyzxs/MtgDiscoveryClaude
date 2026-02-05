---
paths:
  - "csharp/src/Lib.Adapters/Lib.Adapter.*/**"
---

# Adapter Folder Structure

## Canonical Structure

```
Lib.Adapter.{Domain}/
├── Apis/                      # Public contract (required)
│   ├── I{Domain}AdapterService.cs
│   ├── {Domain}AdapterService.cs
│   ├── I{Domain}QueryAdapter.cs    # If has queries
│   ├── I{Domain}CommandAdapter.cs  # If has commands
│   └── Entities/              # XfrEntity interfaces
│       └── I{Operation}XfrEntity.cs
│
├── Commands/                  # Write operations (if needed)
│   ├── I{Operation}Adapter.cs
│   ├── {Operation}Adapter.cs
│   ├── Mappers/               # Command-specific mappers
│   │   ├── I{Source}To{Target}Mapper.cs
│   │   └── {Source}To{Target}Mapper.cs
│   ├── Resolvers/             # Null Object creation
│   │   ├── I{Domain}Resolver.cs
│   │   └── {Domain}Resolver.cs
│   ├── Integrators/           # State merge logic
│   │   ├── I{Domain}Integrator.cs
│   │   └── {Domain}Integrator.cs
│   └── Strategies/            # Cross-cutting concerns (optional)
│       ├── ICosmosRetryStrategy.cs
│       └── CosmosRetryStrategy.cs
│
├── Queries/                   # Read operations (if needed)
│   ├── I{Operation}Adapter.cs
│   ├── {Operation}Adapter.cs
│   ├── Mappers/               # Query-specific mappers
│   │   ├── I{Source}To{Target}Mapper.cs
│   │   └── {Source}To{Target}Mapper.cs
│   └── Entities/              # Concrete XfrEntity (optional)
│       └── {Entity}XfrEntity.cs
│
└── Exceptions/                # Adapter-specific exceptions
    └── {Domain}AdapterException.cs
```

## Folder Purposes

### Apis/ (Required)
The **public contract** for this adapter. Contains:
- Composite service interface and implementation
- Query/Command adapter interfaces (CQRS split)
- XfrEntity interfaces that calling layers create

**Visibility**: Everything here must be `public`.

### Commands/ (Optional)
**Write operation** implementations. Only needed if adapter modifies data.

| Subfolder | Purpose | Required |
|-----------|---------|----------|
| `Mappers/` | Transform XfrEntity → ReadPointItem, etc. | If transformations needed |
| `Resolvers/` | Create Null Object when reads return empty | If read-modify-write |
| `Integrators/` | Merge changes into existing state | If read-modify-write |
| `Strategies/` | Retry logic, circuit breakers | If concurrency handling |

### Queries/ (Optional)
**Read operation** implementations. Only needed if adapter reads data.

| Subfolder | Purpose | Required |
|-----------|---------|----------|
| `Mappers/` | Transform XfrEntity → query args | If transformations needed |
| `Entities/` | Concrete XfrEntity from other adapters | If cross-adapter calls |

### Exceptions/ (Recommended)
Domain-specific exception types. All adapters should have a custom exception.

## Folder Variations by Adapter Type

### Query-Only Adapters
Adapters that only read data (e.g., `Lib.Adapter.Artists`, `Lib.Adapter.Cards`, `Lib.Adapter.Sets`):

```
Lib.Adapter.{Domain}/
├── Apis/
│   ├── Entities/
│   └── ...
├── Queries/
│   ├── Mappers/      # XfrEntity → Inquisition args
│   └── Entities/     # Concrete XfrEntity (if calling other adapters)
└── Exceptions/
```

### Command-Only Adapters
Adapters that only write data (e.g., `Lib.Adapter.User`):

```
Lib.Adapter.{Domain}/
├── Apis/
│   ├── Entities/
│   └── ...
├── Commands/
│   ├── Mappers/
│   ├── Resolvers/
│   └── Integrators/ (if read-modify-write)
└── Exceptions/
```

### Full CQRS Adapters
Adapters with both read and write operations (e.g., `Lib.Adapter.UserCards`, `Lib.Adapter.UserSetCards`):

```
Lib.Adapter.{Domain}/
├── Apis/
│   ├── Entities/
│   └── ...
├── Commands/
│   ├── Mappers/
│   ├── Resolvers/
│   ├── Integrators/
│   └── Strategies/   # If optimistic concurrency
├── Queries/
│   └── Mappers/
└── Exceptions/
```

## When to Create Subfolders

| Subfolder | Create When |
|-----------|-------------|
| `Mappers/` | Any transformation between entity types |
| `Resolvers/` | Command reads data that might not exist |
| `Integrators/` | Command merges changes into existing state |
| `Strategies/` | Operation needs retry/circuit breaker logic |
| `Entities/` (Queries) | Query calls another adapter's Inquisition |

## Anti-Patterns

**Don't:**
- Put Mappers directly in Commands/ or Queries/ (use subfolder)
- Create empty subfolders for future use
- Mix Query and Command adapters in same file
- Put XfrEntity interfaces in Commands/Entities/ (use Apis/Entities/)

**Do:**
- Create subfolders only when needed
- Keep flat structure for simple adapters
- Follow the pattern established by `Lib.Adapter.UserCards`
