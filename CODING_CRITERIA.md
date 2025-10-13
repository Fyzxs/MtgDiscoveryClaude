# Coding Criteria and Patterns

This file documents specific coding patterns and criteria to follow when working on this codebase.

## General Patterns

### Editing Files
- When using the Edit tool, always set replace_all to true when making replacements

### Marker Classes
- Marker classes are abstract classes with no implementation
- Use semicolon syntax for empty class bodies: `public abstract class ClassName : BaseClass;`
- Should be `abstract`, not `sealed`

### Configuration Implementation Pattern
- Constructor takes 2 params: `string parentKey` and `IConfig config`
  - Exception: The root configuration class (e.g., `ConfigCosmosConfiguration`) uses constructor chaining with `MonoStateConfig` and no parentKey
- Chained constructors should be private
- Methods that return JSON structure interfaces: return new classes with this constructor pattern
- Methods that return marker classes: take a `string sourceKey` and the `IConfig`, with `AsSystemType()` returning `config[sourceKey]`
- Methods returning primitive types: do `config[...]` directly with appropriate casting
- Use colon (`:`) as separator when building keys, not period
- Configuration implementation classes should be prefixed with 'Config' (e.g., `ConfigCosmosConfiguration`)
- Internal classes don't need XML documentation comments
- Root configuration uses interface const keys (e.g., `ICosmosConfiguration.CerberusCosmosConfigKey`) instead of parentKey

### Access Modifiers
- Classes outside the Apis folder should be scoped `internal`
- Public classes are only for the Apis folder
- Objects that are not under the Apis folder (or subfolders) should have internal scope

### Testing
See [TESTING_GUIDELINES.md](TESTING_GUIDELINES.md) for specific unit testing patterns and conventions.

### Cosmos-Specific
- `CosmosItem` class needs to be `public class` (not sealed or abstract) because of Cosmos requirements
- Properties in `CosmosItem` need `JsonProperty` attributes with lower_snake_case names

### Code Style
- No greater than sign (`>`) in comparisons - use boundaries as acceptable values instead
- File-scoped namespaces
- No getters/setters (except for specific violations like `OpResponse` and DTOs)
- No primitives, no nulls, no enums (except at boundaries)
- `init` setters for DTO-style classes like `PointItem`
- Use `ConfigureAwait(false)` on all async calls in non-UI projects
- No public statics except:
  - MonoState pattern implementations (e.g., `MonoStateConfig`)
  - `LoggerMessage` source-generated partial methods (structured logging)
  - Framework-required patterns (e.g., factory methods required by libraries)

### Null Handling and Validators
**Validators are Null Boundary Guards:**
- Validators **must** check for null at system boundaries (GraphQL input, external data, etc.)
- Validators return **boolean** results (`Task<bool> IsValid(...)`) indicating validity
- Null checks in validators are **correct and necessary** - they protect the system interior
- Once past validation, code inside the system can assume non-null
- For optional behavior after validation, use Null Object pattern (not null references)

**Example: Correct Validator Pattern**
```csharp
// This is CORRECT - validators check for null at boundaries
public sealed class CollectedItemNotNullValidator : OperationResponseValidator<TArg, TOut>
{
    public sealed class Validator : IValidator<TArg>
    {
        // Checking for null here is correct and expected
        public Task<bool> IsValid(TArg arg) =>
            Task.FromResult(arg.SomeProperty is not null);
    }
}
```

**Null Object Pattern for Interior Code:**
- After validation passes, use Null Object pattern for optional behavior
- Example: `EmptyCollection` instead of `null`, `NoOpLogger` instead of `null`
- Never pass null through validated code paths

### Validation Architecture Pattern

The codebase uses a sophisticated multi-class validation pattern that follows extreme MicroObjects principles. While this may appear verbose at first, it provides significant benefits for testing, maintainability, and type safety.

**Pattern Structure:**

Each validator container consists of multiple small, focused classes:
- **Container Class** (1): Composes multiple validators in sequence
- **Validator Classes** (N): Each implements specific validation logic (NOT Func delegates - maintains OOP)
- **Nested Validator** (1 per validator): Typed behavior for testability
- **Nested Message** (1 per validator): Typed error message (not string - No Primitives principle)

**Example Structure:**
```
CardIdsArgEntityValidatorContainer (1 class)
├── IsNotNullCardIdsArgEntityValidator (3 classes: main + nested validator + nested message)
├── IdsNotNullCardIdsArgEntityValidator (3 classes: main + nested validator + nested message)
├── HasIdsCardIdsArgEntityValidator (3 classes: main + nested validator + nested message)
└── ValidCardIdsArgEntityValidator (3 classes: main + nested validator + nested message)

Total: 13 classes for complete validation
```

**Why This Pattern:**

1. **Test Isolation**: Each validator is independently testable with a single failure reason
2. **Compile-Time Safety**: Error messages are typed classes, not magic strings
3. **Open/Closed Principle**: New validations are new classes; existing code never changes
4. **Simple Tests**: No complex test configuration or string matching required
5. **Clear Failures**: Tests fail for exactly one reason, making debugging straightforward

**Tradeoffs Accepted:**

- More files (appears verbose to newcomers)
- Learning curve for the pattern
- Many small classes instead of few large ones
- File count may seem high

**Alternatives Rejected:**

- **Func delegates**: Would lose type safety and testability
- **Consolidated validator**: Moves complexity to test configuration
- **String messages**: Violates No Primitives principle
- **Inheritance hierarchies**: Adds unnecessary complexity

**Key Insight:**

The "class explosion" is precision, not complexity. Each class does ONE thing, tests ONE thing, and fails for ONE reason. This makes the codebase more maintainable and testable, even though it requires more files.

**When Creating New Validators:**

1. Create a container class that composes validators
2. Each validator should have its own class (not inline Func)
3. Each validator should have a nested Validator class for the logic
4. Each validator should have a nested Message class for the error message
5. Tests should verify each validator independently

**Reference Implementation:**

See `Lib.MtgDiscovery.Entry/Commands/Validators/` for complete examples of this pattern in action.

### NoArgsEntity Pattern

For operations that require no input arguments, use `NoArgsEntity` instead of `void`, `null`, or omitting parameters entirely. This maintains consistency with the `IOperationResponseService<TInput, TOutput>` pattern and follows MicroObjects principles.

**Pattern Definition:**

```csharp
// Lib.MtgDiscovery.Entry/Entities/NoArgsEntity.cs
internal sealed class NoArgsEntity;
```

**When to Use NoArgsEntity:**

Use `NoArgsEntity` as the input parameter type when:
1. An operation retrieves all items without filtering (e.g., `AllSetsAsync()`)
2. An operation requires no input parameters to execute
3. You want to maintain consistency with `IOperationResponseService<TInput, TOutput>`
4. You want to avoid `void` or `null` parameter patterns

**Example Implementation:**

```csharp
// Interface definition
internal interface IAllSetsEntryService : IOperationResponseService<NoArgsEntity, List<ScryfallSetOutEntity>>
{
}

// Service implementation
internal sealed class AllSetsEntryService : IAllSetsEntryService
{
    public async Task<IOperationResponse<List<ScryfallSetOutEntity>>> Execute(NoArgsEntity input)
    {
        // No validation or mapping needed for NoArgsEntity
        IOperationResponse<ISetItemCollectionOufEntity> opResponse =
            await _setDomainService.AllSetsAsync().ConfigureAwait(false);

        if (opResponse.IsFailure)
            return new FailureOperationResponse<List<ScryfallSetOutEntity>>(opResponse.OuterException);

        List<ScryfallSetOutEntity> outEntities =
            await _setItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

        return new SuccessOperationResponse<List<ScryfallSetOutEntity>>(outEntities);
    }
}
```

**Validation and Mapping with NoArgsEntity:**

When using `NoArgsEntity`:
- **Skip Validation**: There are no properties to validate, so validation step is unnecessary
- **Skip Mapping**: There is nothing to map or transform
- **Direct Execution**: Proceed directly to domain service call
- This is the ONLY case where skipping validation/mapping is acceptable

**Why Not Use Other Approaches:**

- **void**: Cannot be used as a type parameter in generic interfaces
- **null**: Violates No Nulls principle; lacks type safety
- **Optional Parameters**: Doesn't work with interface contracts
- **Empty Object with Properties**: Adds unnecessary complexity

**Key Insight:**

`NoArgsEntity` is a type-safe way to express "this operation needs no input" while maintaining architectural consistency. It's a marker type that enables uniform service interfaces without compromising MicroObjects principles.

**Reference Implementation:**

See `Lib.MtgDiscovery.Entry/Queries/Sets/AllSetsEntryService.cs` for a complete example.

### Naming Conventions
- Remove redundant terms from names (e.g., `ICosmosContainerReadOperator` not `ICosmosContainerReadItemOperator`)
- Method names should also avoid redundancy (e.g., `ReadAsync` not `ReadItemAsync`)

### Error Handling
- Use `inheritdoc` tag for override methods

### Pragma Directives
- #pragma directives should be temporary and removed as soon as possible
- Every #pragma warning disable must have a specific justification comment
- Review and remove unnecessary #pragma directives during code reviews
- Prefer fixing the underlying issue over suppressing warnings
- If a #pragma is absolutely necessary, scope it as narrowly as possible (specific line vs entire file)
- Common unnecessary pragmas to watch for:
  - CA1515 (partial classes) - often left after refactoring
  - IDE0055 (formatting) - fix the formatting instead
  - Pragmas suppressing warnings that no longer occur in the code
- During code reviews, verify each #pragma is still needed and justified
