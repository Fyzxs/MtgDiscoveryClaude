# Custom Roslyn Analyzer Proposal

## Problem

Code reviews repeatedly catch the same pattern violations. These 5 rules are documented in `.claude/rules/csharp/csharp-code-style.md` but have no compile-time enforcement:

1. No default values on entity properties (especially ArgEntitys)
2. No `required` keyword on entity properties
3. EntryService classes must implement `IOperationResponseService`
4. GraphQL descriptors must include `Name`
5. No `>` or `>=` operators (reverse to `<` / `<=`)

## Proposed Solution

A solution-local Roslyn analyzer project that catches all 5 rules at build time. Because `TreatWarningsAsErrors` is already enabled, violations become build errors automatically.

## Infrastructure Fit

The existing build infrastructure already supports this:

- `Microsoft.CodeAnalysis` 4.14.0 is in `Directory.Packages.props`
- `RunAnalyzersDuringBuild=true` in `Directory.Build.props`
- `RunAnalyzersDuringLiveAnalysis=true` (IDE feedback)
- `TreatWarningsAsErrors=true` (violations break the build)
- `AnalysisLevel=latest-all`

## Project Structure

### Analyzer Project

**Location**: `common/Lib.Shared.Analyzers/Lib.Shared.Analyzers.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <IsRoslynComponent>true</IsRoslynComponent>
    <EnforceExtendedAnalyzerRules>true</EnforceExtendedAnalyzerRules>
    <IsPackable>false</IsPackable>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.14.0" PrivateAssets="all" />
    <PackageReference Include="Microsoft.CodeAnalysis.Analyzers" PrivateAssets="all" />
  </ItemGroup>
</Project>
```

**Key constraint**: Roslyn analyzers must target `netstandard2.0` regardless of the solution's target framework. This is because the compiler host (VS, Rider, `dotnet build`) may run on different runtimes.

### Wiring Into the Build

Add to `Directory.Build.props`:

```xml
<ItemGroup Condition="!$(MSBuildProjectName.Contains('Analyzer'))">
  <ProjectReference Include="$(MSBuildThisFileDirectory)common\Lib.Shared.Analyzers\Lib.Shared.Analyzers.csproj"
                    OutputItemType="Analyzer"
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

- `OutputItemType="Analyzer"` tells the compiler to load it as an analyzer, not a library reference
- `ReferenceOutputAssembly="false"` prevents runtime dependency on the analyzer DLL
- The `Condition` prevents the analyzer from analyzing itself

### Test Project

**Location**: `common/Lib.Shared.Analyzers.Tests/Lib.Shared.Analyzers.Tests.csproj`

Targets `net10.0` (only the analyzer itself must be `netstandard2.0`). Uses `Microsoft.CodeAnalysis.CSharp.Analyzer.Testing` for the Roslyn test harness.

## Rule Details

### MTG001: No Default Values on Entity Properties

**Severity**: Error
**Roslyn API**: `RegisterSyntaxNodeAction` on `SyntaxKind.PropertyDeclaration`
**Detection**: Check `PropertyDeclarationSyntax.Initializer != null` on classes with names ending in `ArgEntity`, `ExtEntity`, `XfrEntity`, `OufEntity`, `ItrEntity`, or `Entitys`
**Complexity**: Low (pure syntax check)

```csharp
// Violation
public class UserCardExtEntity
{
    public string Name { get; init; } = string.Empty;  // MTG001
}

// Correct
public class UserCardExtEntity
{
    public string Name { get; init; }
}
```

### MTG002: No `required` Modifier on Entity Properties

**Severity**: Error
**Roslyn API**: `RegisterSyntaxNodeAction` on `SyntaxKind.PropertyDeclaration`
**Detection**: Check `property.Modifiers.Any(SyntaxKind.RequiredKeyword)` on entity classes (same suffix matching as MTG001)
**Complexity**: Low (pure syntax check)

```csharp
// Violation
public class AddUserCardArgEntity
{
    public required string CardId { get; init; }  // MTG002
}

// Correct
public class AddUserCardArgEntity
{
    public string CardId { get; init; }
}
```

### MTG003: EntryService Must Implement IOperationResponseService

**Severity**: Error
**Roslyn API**: `RegisterSymbolAction` on `SymbolKind.NamedType`
**Detection**: For classes with names ending in `EntryService`, check `INamedTypeSymbol.AllInterfaces` for `IOperationResponseService`
**Complexity**: Medium (requires semantic model for interface resolution)

```csharp
// Violation
internal sealed class UserEntryService : IUserEntryService  // MTG003
{
}

// Correct
internal sealed class UserEntryService : IUserEntryService
{
    // IUserEntryService extends IOperationResponseService<...>
}
```

**Note**: `AllInterfaces` includes transitively implemented interfaces, so if `IUserEntryService` itself extends `IOperationResponseService`, that satisfies the check.

### MTG004: GraphQL Descriptors Must Include Name

**Severity**: Error
**Roslyn API**: `RegisterSyntaxNodeAction` on `SyntaxKind.MethodDeclaration`
**Detection**: Find `Configure` methods with descriptor parameters (`IObjectTypeDescriptor`, `IInputObjectTypeDescriptor`, etc.), then walk `DescendantNodes()` for a `.Name()` invocation on the descriptor
**Complexity**: Medium-High (requires semantic model for parameter type resolution and handling fluent chains)

```csharp
// Violation
protected override void Configure(IObjectTypeDescriptor<Card> descriptor)
{
    descriptor.Description("A card");  // MTG004: no .Name() call
}

// Correct
protected override void Configure(IObjectTypeDescriptor<Card> descriptor)
{
    descriptor.Name("Card")
        .Description("A card");
}
```

**Implementation considerations**:
- Must handle fluent chaining (`.Name()` could be called on the return of another method)
- Should check `IObjectTypeDescriptor`, `IInputObjectTypeDescriptor`, `IUnionTypeDescriptor`, and `IEnumTypeDescriptor`
- Uses `SemanticModel.GetSymbolInfo()` and `SemanticModel.GetTypeInfo()` to resolve types

### MTG005: No Greater-Than Operators

**Severity**: Warning (with code fix potential)
**Roslyn API**: `RegisterSyntaxNodeAction` on `SyntaxKind.GreaterThanExpression` and `SyntaxKind.GreaterThanOrEqualExpression`
**Detection**: Any `BinaryExpressionSyntax` with those kinds is a violation
**Complexity**: Trivial

```csharp
// Violation
if (count > 100) { }   // MTG005
if (count >= 100) { }  // MTG005

// Correct
if (100 < count) { }
if (100 <= count) { }
```

**Future enhancement**: A `CodeFixProvider` could auto-swap operands and flip the operator.

## Implementation Order

Recommended order based on complexity and value:

| Priority | Rule | Effort | Catches |
|----------|------|--------|---------|
| 1 | MTG005 (no `>`) | Trivial | Frequent style violation |
| 2 | MTG001 (no defaults) | Low | Subtle bugs, especially on ArgEntitys |
| 3 | MTG002 (no `required`) | Low | Style violation |
| 4 | MTG003 (EntryService interface) | Medium | Missing contract implementation |
| 5 | MTG004 (descriptor Name) | Medium-High | GraphQL schema issues |

## Testing Approach

Each analyzer gets tests using the Roslyn testing harness:

```csharp
using Verify = CSharpAnalyzerVerifier<NoDefaultEntityValuesAnalyzer, DefaultVerifier>;

[TestMethod]
public async Task PropertyWithInitializer_OnExtEntity_ReportsDiagnostic()
{
    const string testCode = """
        public class UserCardExtEntity
        {
            public string Name { get; set; } {|#0:= ""|};
        }
        """;

    var expected = Verify.Diagnostic()
        .WithLocation(0)
        .WithArguments("Name", "UserCardExtEntity");

    await Verify.VerifyAnalyzerAsync(testCode, expected);
}
```

Test categories per rule:
- **Positive**: violation detected on matching entity types
- **Negative**: no false positive on non-entity classes
- **Edge cases**: nested classes, partial classes, records, interfaces

## Decisions to Make Before Implementation

1. **Diagnostic ID prefix**: `MTG` is proposed above; pick whatever fits the project conventions
2. **Entity suffix list**: Currently `ArgEntity`, `ExtEntity`, `XfrEntity`, `OufEntity`, `ItrEntity`, `Entitys`. Confirm this is exhaustive.
3. **MTG004 descriptor types**: Which HotChocolate descriptor interfaces should be checked? `IObjectTypeDescriptor`, `IInputObjectTypeDescriptor` at minimum, possibly others.
4. **MTG003 scope**: Should this also check `DomainService`, `AggregatorService`, and adapter service classes, or only `EntryService`?
5. **MTG005 exceptions**: Are there any contexts where `>` is acceptable (e.g., LINQ expressions, generic constraints)?
