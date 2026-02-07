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

## Test Organization

Tests follow the codebase structure:
- **File naming**: `{ProductionClass}Tests`
- **Folder structure**: Mirror production namespace in `*.Tests` projects
- **Test order**: Happy path first, then edge cases

## Learning by Example

Read existing tests to understand the patterns. The code is the documentation.

## Extensive Documentation
While code is the authority, disagreements can be settled with the [../../TESTING_GUIDELINES.md]
