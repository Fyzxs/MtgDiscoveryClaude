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
