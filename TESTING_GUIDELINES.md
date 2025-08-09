# Unit Testing Guidelines

This document outlines the specific patterns and conventions for writing unit tests in this codebase.

## Test Framework and Assertions

- **Framework**: Use MSTest (`[TestClass]`, `[TestMethod]`)
- **Assertions**: Use AwesomeAssertions syntax (e.g., `result.Should().Be(expected)`)
- **Imports**: Import AwesomeAssertions, not FluentAssertions (they have the same syntax)

# Guidelines
- When naming fakes, put "Fake" as a suffix, not prefix (e.g., `ConfigFake`, not `FakeConfig`)
- Fakes should not be in a test class - they should be put into a Fakes folder in the root of the test project
- Do not use [TestInitialize] unless specifically asked for
- Don't use `actual.Should().NotBeNull()` - it's redundant when followed by another assertion that would fail anyway if null
- Unit tests should be wholly self-contained - no test class variables should exist
- Test readability is more important than avoiding duplication - everything should be defined IN the test
- Limit of 3 build failures before stopping to let user fix issues
- When testing abstract classes, create a private implementation within the test class that's used in the tests
- Never modify production code to support test scenarios - tests must work with production code as-is
- When testing protected methods, add a public method to the test implementation that exposes the protected method (e.g., `public ReturnType MethodNameAccess(params) => ProtectedMethod(params);`) instead of using reflection

## Test Structure

### Self-Contained Tests
- Each test must be completely self-contained
- NO test class variables or fields
- ALL test data and dependencies must be created within each test method
- Test readability is more important than avoiding duplication

### Arrange-Act-Assert Pattern
```csharp
[TestMethod]
public async Task MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    // Create all test data and dependencies here
    
    // Act
    // Execute the method under test
    
    // Assert
    // Verify the expected outcomes
}
```

### Act Section Guidelines
- The returned value from the Act section should be named `actual`
- If the returned value is not validated in assertions, assign it to `_` (discard)
- Examples:
  ```csharp
  // When validating the result
  var actual = await subject.MethodAsync(parameter).ConfigureAwait(false);
  
  // When not validating the result
  _ = await subject.MethodAsync(parameter).ConfigureAwait(false);
  ```

### Test Naming Convention
- Format: `MethodName_Scenario_ExpectedBehavior`
- Examples:
  - `DeleteAsync_ShouldCallContainerDeleteItemAsync`
  - `ReadAsync_WhenItemNotFound_ShouldReturnNotFoundResponse`
  - `UpsertAsync_WithValidItem_ShouldReturnSuccessResponse`

## Fake Implementations

### Location and Naming
- Place all fakes in the `Fakes` folder at the root of the test project
- Use "Fake" as a suffix, not prefix (e.g., `CosmosClientFake`, not `FakeCosmosClient`)
- Fakes should be `internal sealed` classes

### Fake Pattern
```csharp
internal sealed class ServiceFake : IService
{
    // Property for return value with init setter
    public string GetValueResult { get; init; }
    
    // Property for tracking invocation count
    public int GetValueInvokeCount { get; private set; }
    
    public string GetValue()
    {
        GetValueInvokeCount++;
        return GetValueResult;
    }
}
```

### LoggerFake Usage
- Use `LoggerFake` without generic type parameter

## Testing Classes with Private Constructors

### TypeWrapper Pattern
When testing a class with a private constructor, use the TypeWrapper pattern from TestConvenience.Core:

```csharp
private sealed class InstanceWrapper : TypeWrapper<ClassUnderTest>
{
    public InstanceWrapper(ILogger logger, IDependency dependency) 
        : base(logger, dependency) { }
}
```

Then create the subject:
```csharp
ClassUnderTest subject = new InstanceWrapper(loggerFake, dependencyFake);
```

### Subject Naming
- Always name the instance under test as `subject`
- This provides consistency across all tests

## Testing Abstract Classes

When testing abstract classes, create a private test implementation within the test class:

```csharp
[TestClass]
public sealed class AbstractClassTests
{
    private sealed class TestableImplementation : AbstractClass
    {
        public TestableImplementation(ILogger logger) : base(logger) { }
        
        // If testing protected methods, expose them:
        public ReturnType ProtectedMethodAccess(params) 
            => ProtectedMethod(params);
    }
}
```

## Async Testing

### ConfigureAwait Usage
- Always use `.ConfigureAwait(false)` on async calls in tests
- This matches production code patterns

### Example
```csharp
[TestMethod]
public async Task DeleteAsync_ShouldSucceed()
{
    // Arrange
    var subject = new InstanceWrapper(dependencies);
    
    // Act
    var result = await subject.DeleteAsync(item).ConfigureAwait(false);
    
    // Assert
    result.Should().NotBeNull();
}
```

## Assertion Guidelines

### Avoid Redundant Assertions
- Don't use `.Should().NotBeNull()` when followed by another assertion that would fail if null
- Example:
  ```csharp
  // Bad
  result.Should().NotBeNull();
  result.Value.Should().Be(expected);
  
  // Good
  result.Value.Should().Be(expected);
  ```

### Invoke Count Assertions
- Always verify fake method invocation counts
- This ensures the correct methods were called the expected number of times
- Example:
  ```csharp
  containerFake.DeleteItemAsyncInvokeCount.Should().Be(1);
  ```

## Test Data

### Use Constants
- Define test data as constants when possible
- Use meaningful names that describe the test scenario

### Example
```csharp
const string itemId = "testId";
const double expectedRequestCharge = 5.5;
PartitionKey partitionKey = new("testPartition");
```

## Common Mistakes to Avoid

1. **Don't use [TestInitialize]** - Each test should set up its own data
2. **Don't modify production code** to support test scenarios
3. **Don't use reflection** to access private members - use TypeWrapper or test implementations
4. **Don't create test base classes** - Keep tests independent
5. **Don't use generic LoggerFake** - Use non-generic `LoggerFake`
6. **Don't forget ConfigureAwait(false)** on async calls

## Test Organization

### One Test Class Per Production Class
- Name test classes as `{ProductionClassName}Tests`
- Place in parallel namespace structure

### Group Related Tests
- Keep tests for the same method together
- Order tests logically (happy path first, then edge cases)

## Example Test Class Structure

```csharp
using System.Net;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConvenience.Core.Fakes;
using TestConvenience.Core.Reflection;

namespace ProjectName.Tests.FolderName;

[TestClass]
public sealed class ClassNameTests
{
    [TestMethod]
    public async Task MethodName_HappyPath_ShouldSucceed()
    {
        // Arrange
        const string testData = "test";
        ServiceFake serviceFake = new() { MethodResult = expectedResult };
        LoggerFake loggerFake = new();
        ClassName subject = new InstanceWrapper(loggerFake, serviceFake);
        
        // Act
        var result = await subject.MethodAsync(testData).ConfigureAwait(false);
        
        // Assert
        result.Should().Be(expectedResult);
        serviceFake.MethodInvokeCount.Should().Be(1);
    }
    
    private sealed class InstanceWrapper : TypeWrapper<ClassName>
    {
        public InstanceWrapper(ILogger logger, IService service) 
            : base(logger, service) { }
    }
}
```