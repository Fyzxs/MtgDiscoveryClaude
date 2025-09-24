# MTG Discovery Vibe - Technical Architecture Manual

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [System Architecture Overview](#system-architecture-overview)
3. [Philosophical Foundation: MicroObjects](#philosophical-foundation-microobjects)
4. [Layered Architecture](#layered-architecture)
5. [Entity Types and Data Flow](#entity-types-and-data-flow)
6. [Core Architectural Patterns](#core-architectural-patterns)
7. [Dependency Injection and Service Location](#dependency-injection-and-service-location)
8. [Configuration Management](#configuration-management)
9. [GraphQL API Design](#graphql-api-design)
10. [Authentication and Authorization](#authentication-and-authorization)
11. [Testing Architecture](#testing-architecture)
12. [Database Integration Patterns](#database-integration-patterns)
13. [External Service Integration](#external-service-integration)
14. [Error Handling and Response Patterns](#error-handling-and-response-patterns)
15. [Development Guidelines and Best Practices](#development-guidelines-and-best-practices)

---

## Executive Summary

The MTG Discovery Vibe platform is a Magic: The Gathering collection management system built on .NET 9.0, implementing an extreme object-oriented architecture pattern called **MicroObjects**. The system provides a GraphQL API for card collection management, integrating with the Scryfall API for card data and Azure Cosmos DB for persistence.

**Key Technical Decisions:**
- **MicroObjects Pattern**: Every concept has explicit object representation with no primitive obsession
- **Strict Layered Architecture**: Unidirectional data flow through well-defined layers
- **GraphQL-First API**: HotChocolate implementation with union types for polymorphic responses
- **JWT Authentication**: Auth0 integration for secure user management
- **Immutable Objects**: All domain objects are immutable with private readonly fields
- **No Nulls Policy**: Null Object pattern used throughout
- **Interface-Driven Design**: 1:1 interface-to-class mapping for all types

The architecture prioritizes explicitness, maintainability, and testability over brevity, resulting in a codebase where every concept is discoverable and every behavior is encapsulated.

---

## System Architecture Overview

### High-Level Architecture

The system follows a multi-tier architecture with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                     GraphQL API Layer                        │
│                 (App.MtgDiscovery.GraphQL)                  │
├─────────────────────────────────────────────────────────────┤
│                      Entry Layer                             │
│                 (Lib.MtgDiscovery.Entry)                    │
├─────────────────────────────────────────────────────────────┤
│                      Shared Layer                            │
│    (Lib.Shared.Abstractions, DataModels, Invocation)       │
├─────────────────────────────────────────────────────────────┤
│                      Domain Layer                            │
│      (Lib.Domain.Cards, Sets, Artists, User)               │
├─────────────────────────────────────────────────────────────┤
│                    Aggregator Layer                          │
│    (Lib.Aggregator.Cards, Sets, Artists, User)             │
├─────────────────────────────────────────────────────────────┤
│                      Adapter Layer                           │
│      (Lib.Adapter.Cards, Sets, Artists, User)              │
├─────────────────────────────────────────────────────────────┤
│                  Infrastructure Layer                        │
│         (Lib.Cosmos, Lib.Universal)                         │
└─────────────────────────────────────────────────────────────┘
```

### Project Organization

The solution is organized into logical groupings:

- **App Layer**: GraphQL API and web hosting
- **Entry Layer**: Service entry points and request validation
- **Shared Layer**: Cross-cutting concerns and shared contracts
- **Domain Layer**: Business logic and domain rules
- **Aggregator Layer**: Data orchestration and aggregation
- **Adapter Layer**: External system integration
- **Infrastructure Layer**: Core utilities and database access
- **Test Projects**: Unit tests following the same structure
- **Example Projects**: Demonstration and exploration applications

---

## Philosophical Foundation: MicroObjects

### Core Principle

> **"Have a representation for every concept that exists in the code"**

MicroObjects is an extreme object-oriented programming approach where every abstract idea, behavior, or data element is represented as a distinct object. This philosophy drives all architectural decisions in the codebase.

### Key Tenets

#### 1. No Primitives (Except in DTOs)
```csharp
// ❌ BAD - Using primitive
public class User {
    public string Id { get; set; }
}

// ✅ GOOD - Domain object wrapping primitive
public interface IUserId {
    string Value();
}

// ✅ PRAGMATIC - DTO with primitives for serialization
public class UserInfoItrEntity : IUserInfoItrEntity {
    public string UserId { get; init; }  // Acceptable in DTOs
}
```

#### 2. Immutability Everywhere
```csharp
public sealed class CardItem : ICardItem {
    private readonly string _cardId;
    private readonly string _cardName;

    public CardItem(string cardId, string cardName) {
        _cardId = cardId;
        _cardName = cardName;
    }

    // Return new instance for any "changes"
    public ICardItem WithName(string newName) {
        return new CardItem(_cardId, newName);
    }
}
```

#### 3. Behavior Over Data
Objects expose what they can do, not what data they contain:

```csharp
// ❌ BAD - Exposing data
public interface ICard {
    string Name { get; }
    int ManaCost { get; }
}

// ✅ GOOD - Exposing behavior
public interface ICard {
    bool CanBeCastWith(IManaPool available);
    ICard Transform();
}
```

#### 4. No Nulls
The Null Object pattern is used throughout:

```csharp
public sealed class UnknownUser : IUser {
    public string Name() => "Unknown";
    public void SendNotification(INotification notification) {
        // No-op - unknown users don't receive notifications
    }
}
```

#### 5. Interface for Every Class
Every class has a corresponding interface defining its contract:

```csharp
public interface ICardEntryService {
    Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args);
}

public sealed class CardEntryService : ICardEntryService {
    // Implementation
}
```

### Pragmatic Adaptations

While maintaining MicroObjects philosophy, the codebase makes pragmatic choices:

1. **DTOs Use Primitives**: For serialization/deserialization simplicity
2. **Entity Suffix Convention**: Clear naming for different entity types
3. **Async/Await Patterns**: Modern C# async patterns are embraced
4. **ConfigureAwait(false)**: Used consistently for library code

---

## Layered Architecture

### Data Flow Overview

The architecture enforces strict unidirectional data flow:

**Request Flow (Inbound):**
1. **App Layer** → Receives GraphQL request, creates ArgEntity
2. **Entry Layer** → Validates ArgEntity, maps to ItrEntity
3. **Shared Layer** → Applies cross-cutting rules
4. **Domain Layer** → Applies business logic
5. **Aggregator Layer** → Orchestrates data retrieval
6. **Adapter Layer** → Maps to ExtEntity, calls external systems

**Response Flow (Outbound):**
1. **Adapter Layer** → Maps ExtEntity back to OufEntity
2. **Aggregator Layer** → Aggregates responses
3. **Domain Layer** → Applies post-processing rules
4. **Shared Layer** → Applies response filters
5. **Entry Layer** → Maps OufEntity to OutEntity
6. **App Layer** → Returns GraphQL response

### Layer Responsibilities

#### App Layer (App.MtgDiscovery.GraphQL)

**Purpose**: GraphQL API endpoint and HTTP hosting

**Key Components:**
- `Startup.cs`: Service configuration and middleware pipeline
- `Queries/`: GraphQL query methods
- `Mutations/`: GraphQL mutation methods
- `Entities/Args/`: Input argument entities (ArgEntity)
- `Entities/Outs/`: Output entities (OutEntity)
- `Entities/Types/`: GraphQL type definitions
- `Schemas/`: Schema extensions and configurations
- `Authentication/`: JWT authentication components

**Example Query Method:**
```csharp
[ExtendObjectType(typeof(ApiQuery))]
public class CardQueryMethods {
    [GraphQLType(typeof(CardResponseModelUnionType))]
    public async Task<ResponseModel> CardsById(CardIdsArgEntity ids) {
        IOperationResponse<ICardItemCollectionItrEntity> response =
            await _entryService.CardsByIdsAsync(ids).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel() {
            Status = new StatusDataModel() {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        // Map and return success response
    }
}
```

#### Entry Layer (Lib.MtgDiscovery.Entry)

**Purpose**: Service entry point, validation, and coordination

**Key Components:**
- `IEntryService`: Aggregate interface for all entry services
- `EntryService`: Main implementation aggregating sub-services
- Service-specific interfaces (`ICardEntryService`, `ISetEntryService`, etc.)
- Validation logic for incoming requests
- Mapping between ArgEntity and ItrEntity

**Entry Service Pattern:**
```csharp
public sealed class EntryService : IEntryService {
    private readonly ICardEntryService _cardEntryService;
    private readonly ISetEntryService _setEntryService;

    public Task<IOperationResponse<ICardItemCollectionItrEntity>>
        CardsByIdsAsync(ICardIdsArgEntity args) =>
            _cardEntryService.CardsByIdsAsync(args);
}
```

#### Shared Layer (Lib.Shared.*)

**Purpose**: Cross-cutting concerns and shared contracts

**Sub-projects:**
- **Lib.Shared.Abstractions**: Core interfaces and action patterns
- **Lib.Shared.DataModels**: Entity interfaces (ItrEntity types)
- **Lib.Shared.Invocation**: Operation response patterns

**Key Patterns:**
```csharp
// Operation Response Pattern
public interface IOperationResponse<out TResponseData> {
    bool IsSuccess { get; }
    bool IsFailure { get; }
    TResponseData ResponseData { get; }
    OperationException OuterException { get; }
}

// Entity Interface Pattern
public interface ICardItemItrEntity {
    string Id { get; }
    string Name { get; }
    // Other properties
}
```

#### Domain Layer (Lib.Domain.*)

**Purpose**: Business logic and domain rules

**Domain Services:**
- `Lib.Domain.Cards`: Card-specific business logic
- `Lib.Domain.Sets`: Set management logic
- `Lib.Domain.Artists`: Artist search and relationships
- `Lib.Domain.User`: User registration and management

**Domain Service Pattern:**
```csharp
public interface ICardDomainService {
    Task<IOperationResponse<ICardItemCollectionItrEntity>>
        CardsByIdsAsync(ICardIdsItrEntity args);
}

public sealed class CardDomainService : ICardDomainService {
    private readonly ICardAggregatorService _aggregator;

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>>
        CardsByIdsAsync(ICardIdsItrEntity args) {
        // Apply business rules before aggregation
        var result = await _aggregator.CardsByIdsAsync(args);
        // Apply business rules after aggregation
        return result;
    }
}
```

#### Aggregator Layer (Lib.Aggregator.*)

**Purpose**: Data orchestration and aggregation

**Aggregator Services:**
- Coordinate multiple adapters
- Combine data from different sources
- Handle caching strategies
- Manage parallel data retrieval

**Aggregation Pattern:**
```csharp
public sealed class CardAggregatorService : ICardAggregatorService {
    private readonly ICardsAdapter _cardsAdapter;
    private readonly IImageAdapter _imageAdapter;

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>>
        CardsByIdsAsync(ICardIdsItrEntity args) {
        // Fetch card data
        var cardsTask = _cardsAdapter.GetCardsAsync(args);
        // Fetch images in parallel
        var imagesTask = _imageAdapter.GetImagesAsync(args);

        await Task.WhenAll(cardsTask, imagesTask);

        // Aggregate results
        return CombineResults(cardsTask.Result, imagesTask.Result);
    }
}
```

#### Adapter Layer (Lib.Adapter.*)

**Purpose**: External system integration

**Adapter Types:**
- `Lib.Adapter.Scryfall.Cosmos`: Cosmos DB persistence
- `Lib.Adapter.Cards/Sets/Artists`: Entity-specific adapters
- `Lib.Adapter.User`: User data persistence

**Adapter Pattern:**
```csharp
public sealed class CardsCosmosAdapter : ICardsAdapter {
    private readonly ICosmosContainerAdapter _container;

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>>
        GetCardsAsync(ICardIdsItrEntity args) {
        try {
            // Map ItrEntity to ExtEntity for Cosmos
            var query = MapToCosmosQuery(args);
            // Execute query
            var results = await _container.QueryAsync(query);
            // Map ExtEntity back to ItrEntity
            return MapToItrEntity(results);
        } catch (CosmosException ex) {
            return new FailureOperationResponse<ICardItemCollectionItrEntity>(
                new OperationException(ex.Message));
        }
    }
}
```

#### Infrastructure Layer

**Purpose**: Core utilities and infrastructure components

**Components:**
- **Lib.Universal**: Core utilities, configuration, service locator
- **Lib.Cosmos**: Azure Cosmos DB integration with MicroObjects patterns

---

## Entity Types and Data Flow

### Entity Type Hierarchy

The system uses a consistent entity naming convention to track data through layers:

1. **ArgEntity**: Argument entities from GraphQL layer
2. **ItrEntity**: Internal transfer entities (shared across layers)
3. **ExtEntity**: External entities (for external systems)
4. **OutEntity**: Output entities for GraphQL responses

### Entity Flow Example

```csharp
// 1. GraphQL receives request with ArgEntity
public class CardIdsArgEntity : ICardIdsArgEntity {
    public List<string> Ids { get; init; }
}

// 2. Entry layer validates and maps to ItrEntity
public interface ICardIdsItrEntity {
    IReadOnlyList<string> Ids { get; }
}

// 3. Flows through Domain and Aggregator as ItrEntity

// 4. Adapter maps to ExtEntity for Cosmos
public class CardCosmosQuery : ICardCosmosQuery {
    public string PartitionKey { get; init; }
    public string QueryText { get; init; }
}

// 5. Results mapped back to ItrEntity
public interface ICardItemItrEntity {
    string Id { get; }
    string Name { get; }
    // Additional properties
}

// 6. App layer maps to OutEntity for GraphQL
public class ScryfallCardOutEntity {
    public string Id { get; init; }
    public string Name { get; init; }
    // GraphQL-specific properties
}
```

### Entity Mapping Strategy

Each layer boundary has explicit mappers:

```csharp
public interface IScryfallCardMapper {
    Task<ScryfallCardOutEntity> Map(ICardItemItrEntity source);
}

public sealed class ScryfallCardMapper : IScryfallCardMapper {
    public async Task<ScryfallCardOutEntity> Map(ICardItemItrEntity source) {
        return new ScryfallCardOutEntity {
            Id = source.Id,
            Name = source.Name,
            // Map additional properties
        };
    }
}
```

---

## Core Architectural Patterns

### 1. Service Locator Pattern

The system uses a custom service locator for dependency resolution:

```csharp
public sealed class ServiceLocator : IServiceLocator {
    private static readonly ConcurrentDictionary<Type, object> s_cached = new();
    private static readonly Dictionary<Type, Func<object>> s_factories = [];

    public static void ServiceRegister<TRegisterType>(Func<object> serviceFactory) {
        Type type = typeof(TRegisterType);
        s_factories.TryAdd(type, serviceFactory);
    }

    public TServiceType GetService<TServiceType>() {
        Type type = typeof(TServiceType);
        bool factoryExists = s_factories.TryGetValue(type, out Func<object> factory);
        if (factoryExists is false)
            throw new ArgumentException($"Service not registered [name={type.Name}].");

        object orAdd = s_cached.GetOrAdd(type, (_) => factory!());
        return (TServiceType)orAdd;
    }
}
```

### 2. Operation Response Pattern

All service methods return `IOperationResponse<T>`:

```csharp
public sealed class SuccessOperationResponse<TResponseData> :
    OperationResponse<TResponseData> {
    public SuccessOperationResponse(TResponseData responseData) : base(responseData) {
        IsSuccess = true;
    }
}

public sealed class FailureOperationResponse<TResponseData> :
    OperationResponse<TResponseData> {
    public FailureOperationResponse(OperationException ex) : base(ex) {
        IsSuccess = false;
    }
}
```

### 3. Configuration Pattern

Singleton configuration with hierarchical keys:

```csharp
public sealed class MonoStateConfig : IConfig {
    private static readonly Semaphore s_setOnce = new(1, 1);
    private static IConfiguration s_configuration;

    public static void SetConfiguration(IConfiguration configuration) {
        try {
            s_setOnce.WaitOne();
            HandleAlreadySet();
            s_configuration = configuration;
        } finally {
            s_setOnce.Release();
        }
    }
}

// Usage in config classes
public sealed class ConfigCosmosAccount : IConfig {
    private readonly IConfig _parent;

    public ConfigCosmosAccount() : this(new MonoStateConfig()) { }

    private ConfigCosmosAccount(IConfig parent) {
        _parent = parent;
    }

    public string Endpoint => _parent["Cosmos:Account:Endpoint"];
}
```

### 4. Marker Class Pattern

Type safety without implementation:

```csharp
// Marker class using semicolon syntax
public abstract class CardId : StringPrimitive;
public abstract class SetCode : StringPrimitive;

// Usage
public interface ICardService {
    Task<ICard> GetCard(CardId id);
    Task<IEnumerable<ICard>> GetCardsInSet(SetCode code);
}
```

### 5. Factory Pattern with Dependency Injection

```csharp
public interface ICardServiceFactory {
    ICardService Create();
}

public sealed class CardServiceFactory : ICardServiceFactory {
    private readonly ILogger _logger;

    public CardServiceFactory(ILogger logger) {
        _logger = logger;
    }

    public ICardService Create() {
        var aggregator = new CardAggregatorService(_logger);
        var domain = new CardDomainService(aggregator);
        return new CardEntryService(domain);
    }
}
```

---

## Dependency Injection and Service Location

### Startup Configuration

Services are registered in `Startup.cs`:

```csharp
public void ConfigureServices(IServiceCollection services) {
    // Singleton services
    services.AddSingleton(_configuration);
    services.AddSingleton<ILogger>(sp =>
        sp.GetRequiredService<ILoggerFactory>().CreateLogger("GraphQL"));

    // Scoped services (per request)
    services.AddScoped<IEntryService, EntryService>();

    // GraphQL configuration
    services
        .AddGraphQLServer()
        .AddApiQuery()
        .AddApiMutation()
        .AddAuthorization();
}
```

### Service Locator Registration

For components not using DI container:

```csharp
// Registration at startup
ServiceLocator.ServiceRegister<ICardAggregatorService>(
    () => new CardAggregatorService(logger));

// Usage in code
var aggregator = ServiceLocator.Instance.GetService<ICardAggregatorService>();
```

### Constructor Injection Pattern

All dependencies injected via constructor:

```csharp
public sealed class CardEntryService : ICardEntryService {
    private readonly ICardDomainService _domainService;
    private readonly ILogger _logger;

    public CardEntryService(ICardDomainService domainService, ILogger logger) {
        _domainService = domainService ?? throw new ArgumentNullException(nameof(domainService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}
```

---

## Configuration Management

### Configuration Hierarchy

Configuration follows a hierarchical structure with colon separators:

```json
{
  "Cosmos": {
    "Account": {
      "Endpoint": "https://localhost:8081",
      "AuthMode": "Sas"
    },
    "Database": {
      "Name": "MtgDiscovery"
    },
    "Containers": {
      "Cards": {
        "Name": "cards",
        "PartitionKey": "/setCode"
      }
    }
  }
}
```

### Configuration Classes

Each configuration level has a dedicated class:

```csharp
// Root configuration
public sealed class ConfigCosmos : IConfig {
    private readonly IConfig _root;

    public ConfigCosmos() : this(new MonoStateConfig()) { }

    public IConfig Account => new ConfigCosmosAccount(_root);
    public IConfig Database => new ConfigCosmosDatabase(_root);
}

// Nested configuration
public sealed class ConfigCosmosAccount : IConfig {
    private readonly IConfig _parent;

    public string Endpoint => _parent["Cosmos:Account:Endpoint"];
    public string AuthMode => _parent["Cosmos:Account:AuthMode"];
}
```

### Environment-Specific Configuration

Configuration loaded based on environment:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) => {
            var env = context.HostingEnvironment;
            config
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
        });
```

---

## GraphQL API Design

### Schema Organization

GraphQL schema organized by feature:

```csharp
// Schema extension pattern
[ExtendObjectType(typeof(ApiQuery))]
public class CardQueryMethods {
    // Card-specific queries
}

[ExtendObjectType(typeof(ApiMutation))]
public class UserMutationMethods {
    // User-specific mutations
}
```

### Union Types for Responses

All queries return union types for success/failure:

```graphql
union CardResponseModel =
    | SuccessDataResponseModel
    | FailureResponseModel

type SuccessDataResponseModel {
    data: [ScryfallCard!]!
}

type FailureResponseModel {
    status: StatusDataModel!
}

type StatusDataModel {
    message: String!
    statusCode: Int!
}
```

### Query Example

```graphql
query GetCardsBySet($setCode: String!) {
    cardsBySetCode(setCode: { code: $setCode }) {
        __typename
        ... on SuccessDataResponseModel {
            data {
                id
                name
                manaCost
                typeLine
            }
        }
        ... on FailureResponseModel {
            status {
                message
                statusCode
            }
        }
    }
}
```

### Object Type Definitions

Each entity has a GraphQL object type:

```csharp
public class ScryfallCardType : ObjectType<ScryfallCardOutEntity> {
    protected override void Configure(
        IObjectTypeDescriptor<ScryfallCardOutEntity> descriptor) {
        descriptor.Name("ScryfallCard");

        descriptor.Field(f => f.Id)
            .Type<NonNullType<StringType>>();

        descriptor.Field(f => f.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(f => f.ImageUris)
            .Type<ImageUrisType>();
    }
}
```

---

## Authentication and Authorization

### JWT Authentication Setup

Auth0 integration configured in `Startup.cs`:

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.Authority = $"https://{_configuration["Auth0:Domain"]}/";
        options.Audience = _configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
```

### Authenticated User Extraction

```csharp
public interface IAuthenticatedUser {
    string UserId { get; }
    string Email { get; }
}

public sealed class AuthenticatedUser : IAuthenticatedUser {
    private readonly ClaimsPrincipal _principal;

    public AuthenticatedUser(ClaimsPrincipal principal) {
        _principal = principal;
    }

    public string UserId => _principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string Email => _principal.FindFirst(ClaimTypes.Email)?.Value;
}
```

### Protected Mutations

```csharp
[ExtendObjectType(typeof(ApiMutation))]
public class UserMutationMethods {
    [Authorize]
    public async Task<ResponseModel> RegisterUser(
        [Service] ClaimsPrincipal claimsPrincipal,
        [Service] ILogger logger) {
        var authUser = new AuthenticatedUser(claimsPrincipal);

        // Process registration with authenticated user context
        var response = await _entryService.RegisterUserAsync(authUser);

        return MapResponse(response);
    }
}
```

### User Registration Flow

1. JWT token validated by middleware
2. Claims principal injected into mutation
3. User information extracted from claims
4. Registration processed through layers
5. User stored in Cosmos DB with generated ID

```csharp
// Generate consistent user ID from Auth0 ID
var userIdNamespace = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
var userId = GuidUtility.Create(userIdNamespace, authUser.Auth0Id);
```

---

## Testing Architecture

### Testing Philosophy

- **Self-contained tests**: No shared state between tests
- **Fakes over mocks**: Simple implementations instead of mocking frameworks
- **TypeWrapper pattern**: For testing classes with private constructors
- **Explicit assertions**: Clear verification of behavior

### Test Structure

```csharp
[TestClass]
public sealed class CardEntryServiceTests {
    [TestMethod]
    public async Task CardsByIdsAsync_ValidIds_ReturnsCards() {
        // Arrange
        var fakeDomainService = new FakeCardDomainService();
        var fakeLogger = new FakeLogger();
        var service = new CardEntryService(fakeDomainService, fakeLogger);
        var args = new CardIdsArgEntity { Ids = ["id1", "id2"] };

        // Act
        var result = await service.CardsByIdsAsync(args).ConfigureAwait(false);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResponseData.Data.Count().Should().Be(2);
        fakeDomainService.CardsByIdsAsyncCallCount.Should().Be(1);
    }
}
```

### Fake Implementations

```csharp
internal sealed class FakeCardDomainService : ICardDomainService {
    public int CardsByIdsAsyncCallCount { get; private set; }
    public ICardIdsItrEntity LastCardsByIdsAsyncArgs { get; private set; }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>>
        CardsByIdsAsync(ICardIdsItrEntity args) {
        CardsByIdsAsyncCallCount++;
        LastCardsByIdsAsyncArgs = args;

        var fakeData = new FakeCardItemCollection();
        return Task.FromResult<IOperationResponse<ICardItemCollectionItrEntity>>(
            new SuccessOperationResponse<ICardItemCollectionItrEntity>(fakeData));
    }
}
```

### TypeWrapper Pattern

For testing classes with private constructors:

```csharp
internal sealed class CardServiceWrapper : TypeWrapper<CardService> {
    public CardServiceWrapper(ICardAggregator aggregator) :
        base(aggregator) { }

    public async Task<ICard> GetCardWrapper(string id) {
        return await Instance.GetCard(id).ConfigureAwait(false);
    }
}
```

### Test Naming Convention

Tests follow the pattern: `MethodName_Scenario_ExpectedBehavior`

```csharp
[TestMethod]
public void Constructor_NullLogger_ThrowsArgumentNullException() { }

[TestMethod]
public async Task CardsByNameAsync_EmptyName_ReturnsFailure() { }

[TestMethod]
public async Task CardsBySetCodeAsync_ValidCode_ReturnsAllCardsInSet() { }
```

---

## Database Integration Patterns

### Cosmos DB Configuration

The system uses Azure Cosmos DB with a MicroObjects wrapper:

```csharp
public interface ICosmosContainerAdapter {
    Task<IOperationResponse<T>> ReadItemAsync<T>(string id, string partitionKey);
    Task<IOperationResponse<IEnumerable<T>>> QueryAsync<T>(string query);
    Task<IOperationResponse<T>> UpsertItemAsync<T>(T item, string partitionKey);
}
```

### Document Models

Cosmos documents use specific item classes:

```csharp
public sealed class CardItem : ICardItem {
    [JsonProperty("id")]
    public string Id { get; init; }

    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("setCode")]
    public string SetCode { get; init; }  // Partition key

    [JsonProperty("_etag")]
    public string ETag { get; init; }
}
```

### Query Patterns

```csharp
public sealed class CardCosmosAdapter : ICardCosmosAdapter {
    private readonly ICosmosContainerAdapter _container;

    public async Task<IOperationResponse<IEnumerable<CardItem>>>
        GetCardsBySetAsync(string setCode) {
        var query = $"SELECT * FROM c WHERE c.setCode = '{setCode}'";

        return await _container.QueryAsync<CardItem>(query)
            .ConfigureAwait(false);
    }
}
```

### Connection Management

Singleton pattern for Cosmos client:

```csharp
public sealed class MonoStateCosmosClientAdapter : ICosmosClientAdapter {
    private static CosmosClient s_client;
    private static readonly object s_lock = new object();

    public CosmosClient GetClient() {
        if (s_client != null) return s_client;

        lock (s_lock) {
            if (s_client == null) {
                s_client = CreateClient();
            }
        }

        return s_client;
    }
}
```

---

## External Service Integration

### Scryfall API Integration

The system integrates with Scryfall for card data:

```csharp
public interface IScryfallApiClient {
    Task<IOperationResponse<ScryfallCard>> GetCardAsync(string id);
    Task<IOperationResponse<ScryfallSet>> GetSetAsync(string code);
}

public sealed class ScryfallApiClient : IScryfallApiClient {
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.scryfall.com";

    public async Task<IOperationResponse<ScryfallCard>> GetCardAsync(string id) {
        try {
            var response = await _httpClient.GetAsync($"{BaseUrl}/cards/{id}")
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode is false) {
                return new FailureOperationResponse<ScryfallCard>(
                    new OperationException($"Scryfall API error: {response.StatusCode}"));
            }

            var json = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var card = JsonConvert.DeserializeObject<ScryfallCard>(json);

            return new SuccessOperationResponse<ScryfallCard>(card);
        } catch (Exception ex) {
            return new FailureOperationResponse<ScryfallCard>(
                new OperationException("Failed to fetch card from Scryfall", ex));
        }
    }
}
```

### Bulk Data Ingestion

For importing Scryfall bulk data:

```csharp
public sealed class ScryfallBulkIngestion : IScryfallBulkIngestion {
    private readonly ICosmosContainerAdapter _container;

    public async Task<IOperationResponse<int>> IngestBulkDataAsync(Stream dataStream) {
        using var reader = new StreamReader(dataStream);
        using var jsonReader = new JsonTextReader(reader);

        var serializer = new JsonSerializer();
        var cards = serializer.Deserialize<List<ScryfallCard>>(jsonReader);

        var tasks = cards.Select(card =>
            _container.UpsertItemAsync(MapToCardItem(card), card.SetCode));

        var results = await Task.WhenAll(tasks).ConfigureAwait(false);

        var successCount = results.Count(r => r.IsSuccess);
        return new SuccessOperationResponse<int>(successCount);
    }
}
```

---

## Error Handling and Response Patterns

### Operation Exception Hierarchy

```csharp
public class OperationException : Exception {
    public string StatusMessage { get; }
    public int StatusCode { get; }

    public OperationException(string message, int statusCode = 500) : base(message) {
        StatusMessage = message;
        StatusCode = statusCode;
    }
}

public class ValidationException : OperationException {
    public ValidationException(string message) : base(message, 400) { }
}

public class NotFoundException : OperationException {
    public NotFoundException(string message) : base(message, 404) { }
}
```

### Error Propagation

Errors propagate through layers maintaining context:

```csharp
public async Task<IOperationResponse<ICardItemItrEntity>> GetCardAsync(string id) {
    try {
        // Attempt to fetch card
        var result = await _adapter.GetCardAsync(id).ConfigureAwait(false);

        if (result.IsFailure) {
            // Wrap and add context
            return new FailureOperationResponse<ICardItemItrEntity>(
                new OperationException(
                    $"Failed to fetch card {id}: {result.OuterException.Message}",
                    result.OuterException));
        }

        return new SuccessOperationResponse<ICardItemItrEntity>(result.ResponseData);
    } catch (Exception ex) {
        // Unexpected error
        return new FailureOperationResponse<ICardItemItrEntity>(
            new OperationException($"Unexpected error fetching card {id}", ex));
    }
}
```

### GraphQL Error Responses

Errors mapped to GraphQL failure types:

```csharp
if (response.IsFailure) {
    return new FailureResponseModel {
        Status = new StatusDataModel {
            Message = response.OuterException.StatusMessage,
            StatusCode = response.OuterException.StatusCode,
            Details = response.OuterException.InnerException?.Message
        }
    };
}
```

---

## Development Guidelines and Best Practices

### Code Organization

#### File Structure
- One class per file
- File name matches class name
- Interfaces in same folder as implementations
- Fakes in `Fakes` subfolder

#### Namespace Convention
```csharp
// Project: Lib.Domain.Cards
// Folder: Apis
// Namespace: Lib.Domain.Cards.Apis
namespace Lib.Domain.Cards.Apis;
```

### Coding Standards

#### Immutability
```csharp
public sealed class Card {
    private readonly string _id;
    private readonly string _name;

    public Card(string id, string name) {
        _id = id;
        _name = name;
    }
}
```

#### Async/Await
```csharp
public async Task<IResult> ProcessAsync() {
    var result = await _service.GetDataAsync()
        .ConfigureAwait(false);  // Always use ConfigureAwait(false)

    return result;
}
```

#### Guard Clauses
```csharp
public void Process(IData data) {
    if (data == null) throw new ArgumentNullException(nameof(data));
    if (data.IsValid is false) return;  // Early return

    // Main logic here
}
```

### Naming Conventions

#### Entities
- `*ArgEntity` - GraphQL input arguments
- `*ItrEntity` - Internal transfer objects
- `*ExtEntity` - External system objects
- `*OutEntity` - GraphQL output objects

#### Services
- `I*Service` - Service interfaces
- `*Service` - Service implementations
- `I*Adapter` - Adapter interfaces
- `*Adapter` - Adapter implementations

#### Tests
- `*Tests` - Test class suffix
- `Fake*` - Fake implementation prefix
- `*Wrapper` - TypeWrapper suffix

### Performance Considerations

#### Parallel Processing
```csharp
public async Task<IEnumerable<ICard>> GetCardsAsync(IEnumerable<string> ids) {
    var tasks = ids.Select(id => GetCardAsync(id));
    var results = await Task.WhenAll(tasks).ConfigureAwait(false);
    return results.Where(r => r.IsSuccess).Select(r => r.ResponseData);
}
```

#### Caching Strategy
```csharp
public sealed class CachedCardService : ICardService {
    private readonly IMemoryCache _cache;
    private readonly ICardService _innerService;

    public async Task<ICard> GetCardAsync(string id) {
        if (_cache.TryGetValue(id, out ICard cached)) {
            return cached;
        }

        var card = await _innerService.GetCardAsync(id).ConfigureAwait(false);
        _cache.Set(id, card, TimeSpan.FromMinutes(5));
        return card;
    }
}
```

### Common Pitfalls to Avoid

1. **Never use null** - Always use Null Object pattern
2. **Avoid primitive obsession** - Wrap primitives in domain objects (except DTOs)
3. **No public statics** - Use dependency injection
4. **No switch statements** - Use polymorphism
5. **No else clauses** - Use early returns
6. **No mutable state** - All objects immutable
7. **No getters/setters** - Expose behavior, not data
8. **No inline new** - Use dependency injection

### Extension Points

#### Adding New Entity Types

1. Define ItrEntity interface in `Lib.Shared.DataModels`
2. Create domain service in appropriate `Lib.Domain.*` project
3. Create aggregator service in `Lib.Aggregator.*`
4. Create adapter in `Lib.Adapter.*`
5. Add entry service method in `Lib.MtgDiscovery.Entry`
6. Create GraphQL query/mutation in `App.MtgDiscovery.GraphQL`

#### Adding New External Services

1. Create adapter interface in `Lib.Adapter.*`
2. Implement adapter with error handling
3. Register in aggregator layer
4. Add configuration if needed
5. Create fake for testing

### Debugging and Troubleshooting

#### Logging Strategy
```csharp
public sealed class CardService : ICardService {
    private readonly ILogger _logger;

    public async Task<ICard> GetCardAsync(string id) {
        _logger.LogInformation($"Fetching card: {id}");

        try {
            var result = await _adapter.GetCardAsync(id).ConfigureAwait(false);

            if (result.IsFailure) {
                _logger.LogWarning($"Failed to fetch card {id}: {result.OuterException.Message}");
            }

            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, $"Unexpected error fetching card {id}");
            throw;
        }
    }
}
```

#### Common Issues

1. **Service not registered**: Check ServiceLocator registration
2. **Null reference**: Ensure Null Object pattern used
3. **Async deadlock**: Check ConfigureAwait(false) usage
4. **GraphQL type errors**: Verify union type definitions
5. **Cosmos connection**: Check configuration keys

---

## Conclusion

The MTG Discovery Vibe architecture represents a principled approach to object-oriented design, where every concept has explicit representation and every behavior is encapsulated. While the MicroObjects pattern creates more verbose code, it results in a system that is:

- **Highly maintainable**: Clear boundaries and responsibilities
- **Testable**: Every component can be tested in isolation
- **Discoverable**: Concepts are explicit, not hidden
- **Extensible**: New features follow established patterns
- **Robust**: Immutability and null safety prevent common bugs

The layered architecture ensures unidirectional data flow, making the system predictable and debuggable. The consistent use of patterns across all layers means developers can quickly understand and contribute to any part of the codebase.

This architecture is particularly well-suited for domain-rich applications where business logic complexity justifies the additional abstraction layers, and where long-term maintainability is prioritized over initial development speed.