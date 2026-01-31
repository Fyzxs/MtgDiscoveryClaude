# GitHub Copilot Instructions - MTG Discovery Vibe

## Project Overview
This is a Magic: The Gathering Collection Tracking site. It's the canonical reference implementation of how software should be built. Precision of implementation of patterns and practices is paramount.

## Tech Stack
- **.NET 10 / C#** (modern style) - GraphQL backend and CLI tools
- **React 19 / Vite / MUI** - Frontend site

## Repo Map
- `/csharp/src` → GraphQL backend and custom CLI Tools
- `/client/web` → Front end site

## Architectural Philosophy: MicroObjects
Every concept that can be named should be represented as an explicit object. This creates:
- Self-documenting code through meaningful types
- High maintainability through low coupling
- Flexibility through composition
- Minimal bugs through simplicity

### Core MicroObjects Practices
1. **Have a representation for every concept** - If you can name it, make it an object
2. **No Getters/Setters** - Expose behavior, not data
3. **Be Immutable** - Use `private readonly` fields, return new instances for changes
4. **Interface Everything** - Every non-primitive-obsession class should have an interface
5. **No Logic in Constructors** - Only assign dependencies to fields
6. **Abstract 3rd Party Code** - Wrap external dependencies
7. **No Public Statics** - Use instance methods instead
8. **No New Inline** - Use dependency injection
9. **Composition Over Inheritance** - Prefer composition
10. **If Only as Guard Clauses** - Early returns only, no branching
11. **No Switch/Else** - Replace with polymorphism
12. **No Boolean Negation** - Use `is false` or explicit methods
13. **No Nulls** - Use Null Object pattern
14. **No Primitives** - Wrap in domain objects
15. **No Enums** - Replace with polymorphic objects
16. **Never Reflection** - Breaks encapsulation
17. **No Type Inspection** - No instanceof/typeof
18. **Sealed by default** - Only abstract for inheritance

### Scoping Rules
- Non-test classes should be `internal` unless in an `Apis` folder
- Everything in the `Apis` folder is `public`

## Hard Architecture Rules
1. **Layer isolation** - Dependencies flow inward only
2. **No framework DI** - Use constructor chains
3. **No AutoMapper** - Explicit mapper classes only
4. **Async always** - Use `ConfigureAwait(false)`
5. **No statics** - Instance methods only (except Null Object pattern)
6. **Interface-first** - Every class has an interface
7. **No GetSet** - Expose behavior, not data
8. **Sealed by default** - Only abstract for inheritance
9. **Immutable** - Use `init` setters, `private readonly` fields
10. **No nulls** - Use Null Object pattern or validation

## Common Red Flags to Avoid
- Classes with >5-7 methods
- Methods with >3-4 lines
- Type checking/instanceof
- Mutable state
- Static methods/properties
- Utility/Manager/Service/Helper classes
- Greater than operators (`>`)
- Boolean negation operator (`!`)

## Key Patterns
- **Constructor Chain** - DI without containers
- **Validator Container** - Many small validator classes
- **Explicit Mappers** - Dedicated mapper classes implementing `ICreateMapper<TSource, TDestination>`
- **Inquisition Pattern** - Parameterized Cosmos queries with strongly typed parameters
- **Null Object Pattern** - No null checks or null handling

## Error Handling
All operations return `IOperationResponse<T>` with proper error context. Errors propagate through layers maintaining context.

## Configuration
Hierarchical, singleton-based configuration using MonoState pattern with colon separators.

## Quick Task Reference
- **Adding a GraphQL query** → See `.github/instructions/csharp-backend.instructions.md`
- **Adding validation** → Entry layer: `*ArgEntityValidator` classes in `*ArgEntityValidatorContainer`
- **Adding a Cosmos query** → Use Inquisition pattern: `*ExtArgs`, `*Inquisition`, `*QueryDefinition`
- **Adding a mapper** → Dedicated class `SourceToDestinationMapper` implementing `ICreateMapper<>`
- **Adding domain logic** → Domain layer service (currently passthrough, ready for logic)
- **Adding React components** → See `.github/instructions/react-frontend.instructions.md`
- **Adding tests** → See `.github/instructions/testing.instructions.md`
- **Security** → No secrets in code, explicit auth on endpoints, restrictive CORS
