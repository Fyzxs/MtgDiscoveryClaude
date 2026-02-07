# Lib.Adapters Layer

## Purpose
Interface with external systems (Cosmos DB, Scryfall API, etc.), transforming between internal representations and external system formats. Adapters are the **last line of defense** — they handle external protocols, error translation, and data transformation.

## Architecture Pattern

**Composite Service → Passthrough Service → Specialized Adapters**
Composite Interface Inheritance Patterns

Expected Pattern
I{Domain}AdapterService implements the I{Domain}QueryAdapter and I{Domain}CommandAdapter
I{Domain}QueryAdapter and I{Domain}CommandAdapter implement well-named-methods
{Domain}QueryAdapter and {Domain}CommandAdapter ctor-chain instantiation of one to many {WellNamedMethod}Adapter
{WellNamedMethod}Adapter implements I{WellNamedMethod}Adapter
I{WellNamedMethod}Adapter implements IOperationResponseService<inputType, outputType>


Example: `Lib.Adapter.Artists/Apis/`
- Interface: `IArtistAdapterService.cs` (composite, inherits all specialized adapters)
- Service: `ArtistAdapterService.cs` (passthrough pattern)
- Specialized adapters: `SearchArtistsAdapter.cs`, `CardsByArtistIdAdapter.cs`, etc.

## Entity Transformation Pipeline

```
XfrEntity (input from Aggregator)  →  [Extract primitives]  →  Cosmos Query/Read
                                                                      ↓
                                                        [External system call]
                                                                      ↓
                                      ExtEntity  ←  [Handle external response]
                                          ↓
                              IOperationResponse<ExtEntity> (to Aggregator)
```

**Entity Types**:
- **XfrEntity** (Transfer): Input from Aggregator layer
- **ExtEntity** (External): Format for/from external systems (Cosmos docs, API responses)

See: `Lib.Adapter.Artists/Apis/Entities/` for XfrEntity definitions

## Key Patterns

**Adapter Example**:
- Complete query: `SearchArtistsAdapter.cs:19-80` (query logic, external call, error handling)

**Constructor Pattern**:
- Public with logger, private with dependencies: `SearchArtistsAdapter.cs:23-25`

**Query Pattern** (Cosmos):
- Inquisition pattern for parameterized queries: `SearchArtistsAdapter.cs:37-46`
- Error response wrapping: `SearchArtistsAdapter.cs:48` (check `IsNotSuccessful()`)

**Mapper Example**:
- Data transformation: `CollectionCardIdToReadPointItemMapper.cs:7-18` (constructor chain + mapping logic)

## Cross-Cutting Patterns

Adapters use action patterns from `Lib.Shared.Abstractions/Actions/`. See: `common/csharp-layer-patterns.md`

**Primary pattern**: `ICreateMapper<TSource, TResult>` for transformations (XfrEntity → query args, etc.)

## Key Rules

1. **Constructor Chain**: Public logger → private dependencies
2. **Async Always**: All I/O operations with `ConfigureAwait(false)`
3. **ICreateMapper for Transforms**: Never inline mapping logic
4. **IOperationResponse**: Return responses, never throw exceptions
5. **No Business Logic**: Translate between systems only; logic belongs in Domain/Aggregator
6. **Error Handling**: Wrap external exceptions in `IOperationResponse`
7. **Entity Types**: Input is `IXfrEntity`, output is `ExtEntity` — never `ItrEntity` or `OufEntity`

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| Service Interface | `I{Domain}AdapterService` | `IArtistAdapterService` |
| Service Implementation | `{Domain}AdapterService` | `ArtistAdapterService` |
| Adapter Interface | `I{Operation}{Domain}Adapter` | `ISearchArtistsAdapter` |
| Adapter Implementation | `{Operation}{Domain}Adapter` | `SearchArtistsAdapter` |
| Mapper Interface | `I{Source}To{Target}Mapper` | `ICollectionCardIdToReadPointItemMapper` |
| Mapper Implementation | `{Source}To{Target}Mapper` | `CollectionCardIdToReadPointItemMapper` |
| Xfr Entity | `I{Entity}XfrEntity` | `IArtistSearchTermXfrEntity` |
| Ext Entity | `{Entity}ExtEntity` | `ArtistNameTrigramDataExtEntity` |

## When Adding a New Adapter

1. Create composite interface in `Apis/`
2. Create passthrough service in `Apis/`
3. Create XfrEntity interface in `Apis/Entities/`
4. Create specialized adapter (query/write logic)
5. Create mappers (XfrEntity → query args, etc.)
6. Register in composite interface

See: `Lib.Adapter.Artists/` for complete example
