# GitHub Copilot Instructions

## Project Context
This is a Magic: The Gathering collection management platform built with .NET 9.0 and React, following the MicroObjects architectural pattern.

## Code Style Requirements

### MicroObjects Philosophy
- Every concept gets an object representation
- Interface for every class (1:1 mapping)
- No nulls - use Null Object pattern
- Immutable objects with `private readonly` fields
- Constructor injection only
- No getters/setters except DTOs with `init`
- No enums, no public statics, no reflection

### C# Specific Rules
- File-scoped namespaces
- `ConfigureAwait(false)` on ALL async calls
- No greater than operators (use `<` only)
- No boolean negation - use `is false`
- `internal` scope outside Apis folder
- `public` scope only in Apis folder

### Code Patterns
- If statements only as guard clauses
- No switch/else - use polymorphism
- No inline object creation - use constructor injection
- No logic in constructors
- No type inspection or casting

## Architecture Layers
Follow the layered architecture data flow:
1. App Layer (GraphQL endpoints)
2. Entry Layer (validation and mapping)
3. Shared Layer (cross-cutting concerns)
4. Domain Layer (business logic)
5. Aggregator Layer (data orchestration)
6. Adapter Layer (external integrations)
7. Infrastructure Layer (core utilities)

## Testing Requirements
- MSTest with AwesomeAssertions
- Self-contained tests (no test class variables)
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- TypeWrapper pattern for private constructors
- Fakes instead of mocks

## Frontend (React)
- Material-UI components only (no Tailwind)
- Use MUI sx props for styling
- App* prefix for custom components
- Follow atomic design (atoms → molecules → organisms)
- Apollo Client for GraphQL

## Important Files
Reference these for patterns:
- CLAUDE.md - Project-specific instructions
- CODING_CRITERIA.md - Coding standards
- microobjects_coding_guidelines.md - MicroObjects philosophy
- TESTING_GUIDELINES.md - Testing patterns

## What NOT to Suggest
- Null checks (except validators/mappers)
- Getters/setters in domain objects
- Public static methods
- Enums or switch statements
- Direct object instantiation with `new`
- Comments unless explicitly needed
- Greater than operators
- Boolean negation with `!`