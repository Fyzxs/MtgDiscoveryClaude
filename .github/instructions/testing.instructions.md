# Testing Guidelines

## Test Basics

- **Framework**: MSTest (`[TestClass]`, `[TestMethod]`)
- **Assertions**: AwesomeAssertions syntax (`.Should().Be(...)`)
- **Subject naming**: Always `subject` for instance under test
- **Pattern**: Arrange-Act-Assert (everything in test method, no class variables)
- **Test naming**: `MethodName_Scenario_ExpectedBehavior`

## Key Rules

| Rule | Details |
|------|---------|
| **Self-contained** | NO test class variables; all data created in method |
| **Fakes folder** | Place all fakes in `Fakes/` at root of test project |
| **Fake naming** | Use suffix: `ConfigFake`, not `FakeConfig` |
| **No TestInitialize** | Each test sets up its own data |
| **ConfigureAwait** | Always use `.ConfigureAwait(false)` on async calls |
| **Invoke counts** | Always verify fake method call counts |
| **No reflection** | Use TypeWrapper or test implementations |
| **No null checks** | Don't use `.Should().NotBeNull()` if followed by other assertions |

## Real Code Examples

### Self-Contained Test Pattern
**File**: `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/DefaultCollectionCreatorTests.cs:17-65`

Shows:
- All setup in Arrange block (no class variables)
- TypeWrapper for private constructor
- Fake configuration in constructor
- Fake invoke count verification
- `ConfigureAwait(false)` on async calls

### Validator Test Pattern
**File**: `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/Validators/CreateCollectionArgEntityValidatorContainerTests.cs:13-34`

Shows:
- Container test validating multiple validators
- Individual validator tests (e.g., lines 334-374)
- Inline fakes for test data
- Testing both valid and invalid scenarios

### Fake Implementation Pattern
**File**: `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/Fakes/CollectionsDomainServiceFake.cs:11-144`

Shows:
- `internal sealed` class suffix "Fake"
- Properties: `*Result` (init), `*InvokeCount` (private set), `*LastArgs`/`*LastEntity` to capture calls
- Increment counters in method implementation
- Return pre-configured results

### TypeWrapper Pattern
**File**: `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/DefaultCollectionCreatorTests.cs:163-166`

Shows:
- Inherits from `TypeWrapper<ClassUnderTest>`
- Takes dependencies in constructor
- Passes to base constructor

## Act Section Guidelines

- Result variable named `actual` when validated
- Use `_` (discard) when result not checked
- **Example**: `var actual = await subject.MethodAsync().ConfigureAwait(false);`

## Assertion Best Practices

**Don't**: Redundant null checks
```csharp
actual.Should().NotBeNull();
actual.Value.Should().Be(expected);  // ← redundant null check above
```

**Do**: Let assertion fail if null
```csharp
actual.Value.Should().Be(expected);  // ← fails on null anyway
```

## Common Mistakes

1. Don't use `[TestInitialize]` — set up in each test
2. Don't modify production code for tests — tests work as-is
3. Don't use reflection — use TypeWrapper or test implementations
4. Don't share test data across tests — each test is isolated
5. Don't use generic `LoggerFake<T>` — use non-generic `LoggerFake`
6. Don't forget `ConfigureAwait(false)` on async calls

## Test Organization

- **Naming**: `{ProductionClass}Tests`
- **Namespace**: Mirror production namespace structure
- **Location**: Parallel to production class location in `*.Tests` projects
- **Order**: Happy path first (lines 16-34), then edge cases (lines 36-72)

## Testing Layers

### App Layer (GraphQL) Tests
- Mock `IEntryService`
- Verify response mapping to `ResponseModel`
- Test success and failure union types

### Entry Layer Tests
- Test validation using validator containers
- Test mapping from `ArgEntity` → `ItrEntity`
- Test mapping from `OufEntity` → `OutEntity`
- Mock domain/aggregator services

### Domain Layer Tests
- Test business logic and invariants
- Mock adapter/aggregator services

### Adapter Layer Tests
- Test external system integration
- Mock external APIs/databases
- Test error handling and response mapping

### Infrastructure Layer Tests
- Test configuration management
- Test low-level utility functions
