---
paths:
  - "**/*.Tests/**"
---

# Testing Guide — Patterns You'll See

This guide documents patterns used in the test suite. These are natural outcomes of good test design, not prescriptive rules.


## Test Structure

Tests in this codebase follow a consistent pattern:

- **Framework**: MSTest (`[TestClass]`, `[TestMethod]`)
- **Assertions**: AwesomeAssertions (`.Should().Be(...)`)
- **Subject variable**: Named `subject` for the instance under test
- **Layout**: Arrange-Act-Assert (each test is self-contained)
- **Naming**: `MethodName_Scenario_ExpectedBehavior` (describes what's being tested)

All setup happens within each test method—no shared test fixtures or class variables. This keeps tests isolated and easy to understand.

## Common Patterns

**Arrange-Act-Assert layout**
```
var subject = new ClassUnderTest(dependencies);
var actual = await subject.MethodAsync().ConfigureAwait(false);
actual.Should().Be(expected);
```

**Fakes location**
All test doubles live in `Fakes/` folder at the test project root. Naming: `ConfigFake`, not `FakeConfig`.

**Verifying fake behavior**
Fakes track call counts and arguments so tests can verify behavior: `fake.InvokeCount.Should().Be(1);`

**Fake tracking property conventions**
Each method on a fake has three tracking properties following this naming pattern:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Fakes/CollectionsDomainServiceFake.cs`

**Property naming:** `{MethodName}Result`, `{MethodName}InvokeCount`, `{MethodName}Last{ParameterName}`
**Access modifiers:** `{ get; init; }` for results (configured at construction), `{ get; private set; }` for tracking (mutated during calls)

**Generic fakes** for common types like `IOperationResponse<T>`:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Fakes/OperationResponseFake.cs`

**Avoiding null checks**
Tests don't assert `.Should().NotBeNull()` before other assertions—let assertions fail if the value is null.

## Code Examples to Reference

See these files for real test patterns in action:

- **Self-contained tests**: `Lib.MtgDiscovery.Entry.Tests/.../DefaultCollectionCreatorTests.cs:17-65`
  - Shows: Arrange-Act-Assert, TypeWrapper for private constructors, fake verification
- **Validator tests**: `Lib.MtgDiscovery.Entry.Tests/.../CreateCollectionArgEntityValidatorContainerTests.cs:13-34`
  - Shows: Testing valid and invalid scenarios, multiple validators
- **Fake implementations**: `Lib.MtgDiscovery.Entry.Tests/.../Fakes/CollectionsDomainServiceFake.cs:11-144`
  - Shows: Tracking method calls, configurable results, immutable setup

## Async & Result Variables

When testing async code:
```csharp
var actual = await subject.MethodAsync().ConfigureAwait(false);
actual.Should().Be(expected);
```

If the result isn't validated, use `_` to discard: `_ = await subject.DoSomething();`

## Test Categories

All test methods include a `TestCategory` attribute:

```csharp
[TestMethod, TestCategory("unit")]
public async Task Execute_WithValidArgs_ReturnsSuccess()
```

Use `"unit"` for all unit tests. This enables filtering tests by category in CI and test explorer.

## Test Organization

Tests follow the codebase structure:
- **File naming**: `{ProductionClass}Tests`
- **Folder structure**: Mirror production namespace in `*.Tests` projects
- **Test order**: Happy path first, then edge cases

### Validator Test File Organization

Validator containers and their individual validators are tested in a single file named `{Container}Tests.cs`. The file contains multiple `[TestClass]` definitions — one for the container and one for each individual validator:

> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/Validators/CreateCollectionArgEntityValidatorContainerTests.cs`

**Individual validator test structure:**
1. Test `{Validator}.Validator.IsValid()` — valid input returns `true`, invalid returns `false`
2. Test `{Validator}.Message.AsSystemType()` — returns expected error message string
3. Each test class may include its own private `Fake` class implementing the arg entity interface

This multi-class-per-file pattern is specific to validators because they are tightly coupled to their container and share the same arg entity type.

## TypeWrapper Pattern

Classes with private constructors (constructor chain DI) require `TypeWrapper<T>` for testing:

> **See:** `csharp/src/testShared/TestConvenience.Core/Reflection/TypeWrapper.cs`

Usage in tests:
> **See:** `csharp/src/Api.MtgDiscovery.GraphQL/Lib.MtgDiscovery.Entry.Tests/Commands/Collections/DefaultCollectionCreatorTests.cs`

**How it works:**
- `TypeWrapper<T>` uses `PrivateCtor<T>` (reflection) to invoke the private constructor
- Inherits `ToSystemType<T>` which provides implicit conversion to `T`
- Allows injecting test fakes without exposing the private constructor

**Location**: `testShared/TestConvenience.Core/Reflection/TypeWrapper.cs`

**When to use**: Any class that follows the constructor chain pattern (`public(ILogger)` → `private(dependencies)`) needs a `TypeWrapper` in its test class.

## Test Coverage Expectations

Priority areas for test coverage:
- **Validators and ValidatorContainers** -- verify both valid and invalid scenarios
- **Mappers** -- verify correct transformation between entity types
- **Domain/Entry services** -- verify orchestration logic
- **Enrichments** -- verify post-query data enrichment behavior

Tests mirror the production folder structure in `*.Tests` projects.

## Learning by Example

Read existing tests to understand the patterns. The code is the documentation.

## Extensive Documentation
While code is the authority, disagreements can be settled with the [../../TESTING_GUIDELINES.md]
