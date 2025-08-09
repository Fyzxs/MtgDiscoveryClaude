# Technology Steering Document

## Core Technology Stack

### Backend Platform
- **Language**: C# (.NET 9.0)
- **Architecture**: MicroObjects (extreme OOP pattern)
- **API**: GraphQL via Hot Chocolate
- **Testing**: MSTest with AwesomeAssertions
- **Package Management**: NuGet with central package management

### Frontend Platform (Planned)
- **Framework**: React 18+ with TypeScript 5+
- **Build Tool**: Vite (latest)
- **Styling**: Tailwind CSS
- **Component Library**: shadcn/ui
- **State Management**: TBD based on needs (likely Zustand or built-in Context)
- **API Client**: GraphQL client (Apollo or URQL)

### Data & Storage
- **Database**: Azure CosmosDB
  - Document-based storage for flexibility
  - Partition strategy optimized for query patterns
  - RU consumption optimized for cost (slow is acceptable)
- **File Storage**: Azure Blob Storage
  - Card images in multiple resolutions
  - Hierarchical organization by set/card
- **Caching**: MonoStateMemoryCache (in-memory)
  - No distributed caching needed (single-user primary focus)

### Infrastructure
- **Hosting**: Azure Container Apps (preferred) or App Service
  - Cost-optimized tier selection
  - Single region deployment initially
- **Authentication**: Azure Entra ID
- **Monitoring**: Application Insights (basic tier)
- **CI/CD**: GitHub Actions
- **IaC**: Terraform or Bicep (future phase)

## Architectural Patterns

### MicroObjects Principles (Strict Adherence)
```csharp
// Every concept is a type
public sealed class CardName : ToSystemType<string> { }

// No primitives exposed
public interface ICard 
{
    CardName Name();
    SetCode Set();
}

// Immutable objects only
public sealed class Card : ICard
{
    private readonly CardName _name;
    private readonly SetCode _set;
    
    public Card(CardName name, SetCode set)
    {
        _name = name;
        _set = set;
    }
}
```

### Configuration Pattern
```csharp
// MonoState configuration
MonoStateConfig.SetConfiguration(configuration);

// Nested configuration access
public ICosmosAccountConfig AccountConfig(IDefinition definition)
{
    string key = $"{ParentKey}:{definition.Name().AsSystemType()}";
    return new ConfigCosmosAccountConfig(key, _config);
}
```

### Operator Pattern for Data Access
- **Gopher**: Read operations
- **Scribe**: Write operations
- **Janitor**: Delete operations
- **Inquisitor**: Query operations

### Testing Patterns
```csharp
// Fakes over mocks
internal sealed class ServiceFake : IService
{
    public string MethodResult { get; init; }
    public int MethodInvokeCount { get; private set; }
    
    public string Method()
    {
        MethodInvokeCount++;
        return MethodResult;
    }
}

// TypeWrapper for private constructors
private sealed class Wrapper : TypeWrapper<ClassUnderTest>
{
    public Wrapper(ILogger logger) : base(logger) { }
}
```

## Technology Decisions

### Why .NET 9.0?
- Latest LTS-track version with performance improvements
- Excellent Azure integration
- Strong typing aligns with MicroObjects philosophy
- Comprehensive testing ecosystem

### Why GraphQL?
- Flexible querying for complex collection scenarios
- Strong typing throughout the stack
- Efficient data fetching for nested relationships
- Modern API approach that demonstrates best practices

### Why Azure?
- Enterprise-grade cloud platform
- Excellent .NET integration
- Comprehensive service offering
- Professional deployment patterns for demonstration

### Why MonoState Pattern?
- Simplifies dependency injection in MicroObjects architecture
- Maintains single instance without complex DI container
- Clear, testable pattern

### Why No Distributed Caching?
- Primary single-user focus doesn't require it
- Reduces complexity and cost
- In-memory caching sufficient for use case

## Technical Constraints

### Performance Requirements
- **Response Times**: Sub-second for cached operations
- **Throughput**: Single-user focused, no high-concurrency requirements
- **Data Volume**: Support for 100,000+ cards in collection
- **Memory**: 500MB maximum for cache

### Cost Constraints
- **Priority**: Minimize Azure costs during development/personal use
- **Acceptable Trade-offs**: Slower performance for lower cost
- **Target**: < $50/month for personal use deployment

### Integration Constraints
- **Scryfall API**: Maximum 10 requests/second
- **No API abuse**: Respectful rate limiting and caching
- **HTTPS only**: All external communications encrypted

### Development Constraints
- **No primitive types**: Everything wrapped in domain objects
- **No nulls**: Null Object pattern required
- **No public statics**: Except for MonoState pattern
- **No mocks**: Fakes only in testing
- **100% test coverage**: For business logic

## Code Quality Standards

### Compiler Settings (Directory.Build.props)
```xml
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
<Nullable>warnings</Nullable>
<AnalysisLevel>latest-all</AnalysisLevel>
<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
```

### Required Analyzers
- Microsoft.CodeAnalysis.BannedApiAnalyzers
- AwesomeAssertions.Analyzers (for tests)

### Code Style Rules
- File-scoped namespaces required
- No greater-than operators (use `<` only)
- No boolean negation operator `!` (use `is false`)
- `ConfigureAwait(false)` on all async calls
- `init` setters for DTO properties

## Third-Party Dependencies

### Backend Dependencies
- **Azure.Storage.Blobs**: Blob storage operations
- **Microsoft.Azure.Cosmos**: CosmosDB operations
- **HotChocolate**: GraphQL server
- **Microsoft.Extensions.Caching.Memory**: Caching
- **Newtonsoft.Json**: JSON operations (Cosmos requirement)

### Testing Dependencies
- **MSTest.TestFramework**: Test framework
- **AwesomeAssertions**: Assertion library
- **coverlet.collector**: Code coverage

### Frontend Dependencies (Planned)
- **react**: UI framework
- **typescript**: Type safety
- **tailwindcss**: Styling
- **@radix-ui**: shadcn/ui foundation
- **graphql**: API communication

## Security Considerations

### Authentication & Authorization
- Azure Entra ID for user authentication
- Role-based access control (future)
- No local user management

### Data Protection
- All data encrypted at rest (Azure default)
- HTTPS only for all communications
- No sensitive data in logs
- Configuration secrets in environment variables

### API Security
- Rate limiting on all endpoints
- Input validation on all operations
- No direct database access from frontend

## Monitoring & Observability

### Logging Strategy
- Structured logging throughout
- Error context includes full details
- No sensitive data in logs
- Log retention: 30 days

### Metrics Collection
- API response times
- Cache hit ratios
- Error rates by category
- Ingestion success/failure counts

### Health Checks
- Database connectivity
- External API availability
- Cache operation
- Storage accessibility

## Development Tools

### Required Tools
- Visual Studio 2022 or VS Code with C# extensions
- .NET 9.0 SDK
- Node.js 18+ (for frontend)
- Azure CLI (for deployment)
- Git

### Recommended Tools
- Azure Storage Explorer
- Cosmos DB Explorer
- Postman/Insomnia for API testing
- Docker Desktop (for local development)

## Future Technology Considerations

### Potential Additions
- Redis (if distributed caching needed)
- Azure Functions (for background processing)
- SignalR (for real-time updates)
- Azure Service Bus (for event processing)

### Explicitly Rejected
- Microservices (unnecessary complexity)
- Kubernetes (overkill for use case)
- NoSQL alternatives to Cosmos (Azure integration)
- ORMs (conflicts with MicroObjects philosophy)

## Technology Guidelines

When introducing new technology:
1. Must align with MicroObjects philosophy
2. Should reduce complexity, not add it
3. Must have clear testing strategy
4. Should follow established patterns
5. Must be justifiable for single-user scenario first

Cost and simplicity take precedence over performance and scale during the personal use phase.