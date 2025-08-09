# Structure Steering Document

## Solution Organization

### Root Structure
```
MtgDiscoveryVibe/
├── .claude/                    # Claude AI specifications and steering
│   ├── specs/                  # Feature specifications
│   ├── steering/               # Steering documents (this file)
│   └── templates/              # Spec templates
├── src/                        # All source code
│   ├── Lib.*/                  # Infrastructure libraries
│   ├── Api.*/                  # API implementations
│   ├── Example.*/              # Example applications
│   ├── *.Tests/                # Test projects
│   └── MtgDiscoveryVibe.sln    # Solution file
├── frontend/                   # React application (future)
├── infrastructure/             # IaC templates (future)
├── docs/                       # Additional documentation
├── CODING_CRITERIA.md          # Project-specific patterns
├── TESTING_GUIDELINES.md       # Testing conventions
├── microobjects_coding_guidelines.md  # Architecture philosophy
├── PRD.md                      # Product requirements
├── README.md                   # Project overview
└── CLAUDE.md                   # AI assistant guidance
```

## Project Naming Conventions

### Library Projects
- **Lib.{Feature}**: Infrastructure libraries
  - `Lib.Universal` - Core utilities
  - `Lib.Cosmos` - CosmosDB integration
  - `Lib.BlobStorage` - Blob storage integration
  - `Lib.Scryfall` - Scryfall API client (to be created)

### API Projects
- **Api.{Protocol}**: API implementations
  - `Api.GraphQL` - GraphQL API server (to be created)
  - `Api.Rest` - REST endpoints if needed (future)

### Example Projects
- **Example.{Feature}**: Demonstration applications
  - `Example.Core` - Shared example infrastructure
  - `Example.ScryfallIngestion` - Demonstrates Scryfall integration
  - `Example.{Feature}` - One per major feature

### Test Projects
- **{ProjectName}.Tests**: Unit tests for each project
  - Located adjacent to source project
  - Contains Fakes/ folder for test doubles
  - Follows same namespace structure as source

## Folder Structure Within Projects

### Standard Library Structure (Lib.*)
```
Lib.{Feature}/
├── Adapters/                   # External service adapters
│   ├── I{Service}Adapter.cs    # Adapter interfaces
│   └── MonoState{Service}Adapter.cs  # Singleton implementations
├── Apis/                       # Public API surface
│   ├── Configurations/         # Configuration interfaces and classes
│   │   ├── I{Feature}Config.cs
│   │   └── Config{Feature}Config.cs
│   ├── Ids/                    # Identity value objects
│   │   └── {Concept}Id.cs
│   ├── Operators/              # Operation implementations
│   │   ├── {Feature}Gopher.cs     # Read operations
│   │   ├── {Feature}Scribe.cs     # Write operations
│   │   ├── {Feature}Janitor.cs    # Delete operations
│   │   └── {Feature}Inquisitor.cs # Query operations
│   └── I{Concept}.cs           # Public interfaces
├── Configurations/             # Internal configuration implementations
├── Operations/                 # Internal operation implementations
└── {Feature}.csproj            # Project file
```

### Test Project Structure
```
{ProjectName}.Tests/
├── Fakes/                      # Test doubles
│   ├── {Service}Fake.cs        # Fake implementations
│   └── {Value}Fake.cs          # Value object fakes
├── Adapters/                   # Adapter tests
│   └── {Adapter}Tests.cs
├── Apis/                       # Public API tests
│   ├── Configurations/         # Configuration tests
│   └── Operators/              # Operator tests
├── Configurations/             # Internal configuration tests
├── Operations/                 # Internal operation tests
└── {ProjectName}.Tests.csproj  # Test project file
```

### Example Project Structure
```
Example.{Feature}/
├── Program.cs                  # Entry point
├── {Feature}Application.cs     # Main application class
├── Examples/                   # Individual example scenarios
│   ├── {Scenario}Example.cs
│   └── {Operation}Demo.cs
├── appsettings.json            # Configuration
└── Example.{Feature}.csproj    # Project file
```

## File Naming Conventions

### Classes and Interfaces
- **Interfaces**: `I{Concept}.cs` - One interface per file
- **Implementations**: `{Concept}.cs` - Matches interface name
- **Configurations**: `Config{Feature}{Concept}.cs` - Prefixed with Config
- **Fakes**: `{Concept}Fake.cs` - Suffixed with Fake
- **Tests**: `{Class}Tests.cs` - Suffixed with Tests

### Value Objects (Domain Objects)
- **Identifiers**: `{Concept}Id.cs` (e.g., `CardId.cs`)
- **Names**: `{Concept}Name.cs` (e.g., `SetName.cs`)
- **Values**: `{Concept}Value.cs` (e.g., `ManaValue.cs`)
- **Marker Classes**: Abstract classes with semicolon syntax

### Operators (Data Access)
- **Read**: `{Feature}Gopher.cs` or `{Feature}ReadOperator.cs`
- **Write**: `{Feature}Scribe.cs` or `{Feature}WriteOperator.cs`
- **Delete**: `{Feature}Janitor.cs` or `{Feature}DeleteOperator.cs`
- **Query**: `{Feature}Inquisitor.cs` or `{Feature}QueryOperator.cs`

## Namespace Conventions

### Standard Pattern
```csharp
namespace Lib.{Feature}.Apis.Operators;  // File-scoped namespace
```

### Test Namespaces
```csharp
namespace Lib.{Feature}.Tests.Apis.Operators;  // Mirrors source structure
```

### Example Namespaces
```csharp
namespace Example.{Feature}.Examples;
```

## Code Organization Rules

### Scope Visibility
- **Public**: Only in `Apis/` folders and subfolders
- **Internal**: Everything else
- **Private**: Implementation details within classes
- **Protected**: Only for test accessibility (with TypeWrapper)

### File Organization
- One type per file (interface or class)
- Related types in same folder
- Folder hierarchy mirrors namespace hierarchy
- Configuration interfaces with implementations

### Project References
```xml
<!-- Library references -->
<ProjectReference Include="..\Lib.Universal\Lib.Universal.csproj" />

<!-- Test project references -->
<ProjectReference Include="..\{ProjectName}\{ProjectName}.csproj" />

<!-- InternalsVisibleTo for testing -->
<InternalsVisibleTo Include="{ProjectName}.Tests" />
```

## MTG Domain Structure (Future)

### Card Domain
```
Lib.Cards/
├── Apis/
│   ├── ICard.cs
│   ├── ICardSet.cs
│   ├── Ids/
│   │   ├── CardId.cs
│   │   ├── SetCode.cs
│   │   └── CollectorNumber.cs
│   └── Attributes/
│       ├── CardFinish.cs
│       ├── CardCondition.cs
│       └── CardLanguage.cs
```

### Collection Domain
```
Lib.Collection/
├── Apis/
│   ├── ICollectionItem.cs
│   ├── ICollectionQuantity.cs
│   └── Tracking/
│       ├── QuantityMatrix.cs
│       └── SpecialAttributes.cs
```

### Deck Domain
```
Lib.Decks/
├── Apis/
│   ├── IDeck.cs
│   ├── IDeckCard.cs
│   └── Formats/
│       ├── IFormat.cs
│       └── StandardFormat.cs
```

## Frontend Structure (React - Future)

### Component Organization
```
frontend/
├── src/
│   ├── components/             # Reusable components
│   │   ├── ui/                 # shadcn/ui components
│   │   └── common/             # App-specific components
│   ├── features/               # Feature-specific components
│   │   ├── collection/
│   │   ├── deck-builder/
│   │   └── card-search/
│   ├── hooks/                  # Custom React hooks
│   ├── lib/                    # Utilities and helpers
│   ├── pages/                  # Page components
│   ├── services/               # API clients
│   └── types/                  # TypeScript types
```

## Configuration File Structure

### Backend Configuration
```
appsettings.json
├── Cosmos                      # CosmosDB settings
├── BlobStorage                 # Blob storage settings
├── Scryfall                    # API settings
├── Caching                     # Cache TTL settings
└── Logging                     # Log levels
```

### Environment-Specific
- `appsettings.Development.json` - Local development
- `appsettings.Production.json` - Production settings
- Environment variables for secrets

## Build and Output Structure

### Build Outputs
```
{Project}/
├── bin/
│   ├── Debug/
│   │   └── net9.0/
│   └── Release/
│       └── net9.0/
└── obj/                        # Build intermediates (gitignored)
```

### Package Management
- Central package management via `Directory.Packages.props`
- Version management in single location
- Consistent versions across all projects

## Documentation Structure

### Code Documentation
- XML documentation for public APIs in `Apis/` folders
- No XML documentation for internal classes
- Test names as documentation

### Markdown Documentation
- Feature specs in `.claude/specs/{feature}/`
- Architecture decisions in `docs/decisions/`
- API documentation generated from XML comments

## Example App Patterns

### Structure Requirements
1. Inherit from `ExampleApplication` base class
2. Override `Run()` method for main logic
3. Use constructor for dependency setup
4. Provide clear console output showing operations
5. Include error handling with informative messages

### Example Pattern
```csharp
public sealed class ScryfallIngestionApplication : ExampleApplication
{
    protected override void Run()
    {
        Console.WriteLine("Starting Scryfall Ingestion Example...");
        
        // Demonstrate specific functionality
        Console.WriteLine($"Fetched card: {card.Name().AsSystemType()}");
        
        Console.WriteLine("Example completed successfully!");
    }
}
```

## Conventions Summary

### Must Follow
- MicroObjects patterns throughout
- File-scoped namespaces
- One type per file
- Fakes in test projects
- Example apps for features

### Must Avoid
- Primitives in public APIs
- Public classes outside Apis/
- Multiple types per file
- Mocking frameworks
- Logic in constructors

### Naming Patterns
- Suffix fakes with `Fake`
- Suffix tests with `Tests`
- Prefix configs with `Config`
- Prefix interfaces with `I`
- Use domain-specific names over generic ones