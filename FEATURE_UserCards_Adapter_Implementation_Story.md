# FEATURE UserCards Adapter Implementation Story

## Overview
Implementation of UserCards adapter layer for managing user card collections in Cosmos DB. This adapter will handle write operations (Scribe) for adding cards to a user's collection, following the established CQRS pattern with Commands folder structure.

**Total Tasks**: 23 tasks (11 implementation + 12 test tasks)

## Context
UserCards Cosmos schema has been defined:
```
UserCards
{
  partition: userId
  id: cardId
  cardId: card.Id
  userId: user.Id
  setId: card.set.Id
  collected: [
    {
      finish: {nonfoil|foil|etched}
      special: {none|altered|signed|proof}
      count: {count}
    }
  ]
}
```

The adapter layer follows established patterns from existing adapters (Cards, Artists, Sets, User) and integrates with the Lib.Cosmos infrastructure layer.

## Reference Projects
- **Pattern Reference**: `src/Lib.Adapter.User/` - Use as primary template for adapter structure
- **CQRS Reference**: `src/Lib.Adapter.Cards/` - Reference for CQRS folder structure (Queries folder)
- **Cosmos Integration**: `src/Lib.Adapter.Scryfall.Cosmos/` - Where Cosmos Items and operators live
- **Infrastructure**: `src/Lib.Cosmos/` - Core Cosmos infrastructure components

## User Story 1: Create UserCards Adapter Project Structure

### Acceptance Criteria
- New project Lib.Adapter.UserCards created with proper folder structure
- Project references configured correctly
- Project builds successfully

### Task 1.1: Create Project and Folder Structure

**Location**: `src/Lib.Adapter.UserCards/`

**Reference**: Copy structure from `src/Lib.Adapter.User/`

**Steps**:
1. Create new directory `src/Lib.Adapter.UserCards/`
2. Create subdirectories:
   - `Apis/` - Public interfaces
   - `Commands/` - CQRS command implementations
   - `Entities/` - Response entities if needed
   - `Exceptions/` - Custom exceptions

**Modifications** (describe changes, do not show code):
- No Persistence folder (using Commands instead per CQRS)
- Add Commands folder for CQRS pattern
- Keep Exceptions folder for custom exceptions

### Task 1.2: Unit Tests for Project Structure Creation

**Parent Implementation Task**: Task 1.1 - Create Project and Folder Structure

**Purpose**: Verify project structure and folder creation following TESTING_GUIDELINES.md patterns.

**Test File Location**: Manual verification task - no specific test file needed for folder structure

**Testing Strategy**:
- Verify directory structure matches expected pattern
- Confirm all required folders exist (Apis, Commands, Entities, Exceptions)
- Validate folder structure follows established adapter patterns

**Reference Patterns**:
- Compare structure with `src/Lib.Adapter.User/` folder organization
- Review CQRS folder patterns from `src/Lib.Adapter.Cards/`
- Follow directory conventions from existing adapter projects

**Validation Approach**:
- Directory existence verification
- Structure compliance with adapter patterns
- CQRS folder organization validation

### Task 1.3: Create Project File

**Location**: `src/Lib.Adapter.UserCards/Lib.Adapter.UserCards.csproj`

**Reference**: Copy from `src/Lib.Adapter.User/Lib.Adapter.User.csproj`

**Pattern to Follow**:
- Target framework net9.0
- Add project references to:
  - Lib.Adapter.Scryfall.Cosmos
  - Lib.Shared.DataModels
  - Lib.Shared.Invocation
- Set TreatWarningsAsErrors to true
- Add InternalsVisibleTo for test project

**Modifications**:
- Change RootNamespace to Lib.Adapter.UserCards
- Add reference to Lib.Adapter.Scryfall.Cosmos project
- Keep all other settings identical

### Task 1.4: Unit Tests for Project File Configuration

**Parent Implementation Task**: Task 1.3 - Create Project File

**Purpose**: Verify project file configuration and build settings following TESTING_GUIDELINES.md patterns.

**Test File Location**: Build verification task - validated through successful compilation

**Testing Strategy**:
- Verify project builds successfully
- Confirm all project references are correctly configured
- Validate target framework and compiler settings

**Reference Patterns**:
- Compare configuration with `src/Lib.Adapter.User/Lib.Adapter.User.csproj`
- Review project reference patterns from existing adapter projects
- Follow build configuration from established projects

**Validation Approach**:
- Successful dotnet build execution
- Project reference resolution verification
- Compiler warning/error absence validation
- InternalsVisibleTo configuration verification

## User Story 2: Define UserCards Adapter Interfaces

### Acceptance Criteria
- Main composite interface IUserCardsAdapterService defined
- Specialized interface IUserCardsCommandAdapter defined
- Interfaces follow established patterns with proper documentation

### Task 2.1: Create Main Adapter Interface

**Location**: `src/Lib.Adapter.UserCards/Apis/IUserCardsAdapterService.cs`

**Reference**: Copy from `src/Lib.Adapter.User/Apis/IUserAdapterService.cs:1-30`

**Pattern to Follow**:
- Composite interface pattern inheriting from specialized interfaces
- XML documentation explaining design decisions
- Public interface that will be consumed by aggregator layer

**Modifications**:
- Replace "User" with "UserCards" in interface name
- Update documentation to reference UserCards operations
- Inherit from IUserCardsCommandAdapter instead of IUserPersistenceAdapter
- Update XML comments to describe card collection operations

### Task 2.2: Unit Tests for Main Adapter Interface

**Parent Implementation Task**: Task 2.1 - Create Main Adapter Interface

**Purpose**: Create comprehensive unit tests for IUserCardsAdapterService interface following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Apis/IUserCardsAdapterServiceTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:13-30`
- Review interface testing patterns from existing adapter test projects
- Follow interface validation from `src/Lib.Shared.Abstractions.Tests/`

**Testing Strategy**:
- Interface inheritance verification
- Method signature validation
- XML documentation presence confirmation
- Public interface accessibility verification

**Test Scenarios to Cover**:
- Interface inheritance from IUserCardsCommandAdapter
- Method signatures match expected patterns
- Interface accessibility and visibility
- XML documentation completeness

**Pattern References**:
- Interface testing patterns: Reference interface tests in existing projects
- Test structure: Follow patterns from `src/Lib.Aggregator.Cards.Tests/`
- Naming conventions: Use established interface testing patterns

### Task 2.3: Create Command Adapter Interface

**Location**: `src/Lib.Adapter.UserCards/Apis/IUserCardsCommandAdapter.cs`

**Reference**: Copy from `src/Lib.Adapter.User/Apis/IUserPersistenceAdapter.cs:1-29`

**Pattern to Follow**:
- Specialized interface for command operations
- Uses IOperationResponse from Lib.Shared.Invocation
- Accepts ItrEntity parameters following MicroObjects principles
- Returns ItrEntity wrapped in IOperationResponse

**Modifications**:
- Change interface name from IUserPersistenceAdapter to IUserCardsCommandAdapter
- Replace RegisterUserAsync method with AddUserCardAsync method
- Method signature should accept IUserCardItrEntity parameter
- Return type should be IOperationResponse<IUserCardItrEntity>
- Update XML documentation to describe command operations

### Task 2.4: Unit Tests for Command Adapter Interface

**Parent Implementation Task**: Task 2.3 - Create Command Adapter Interface

**Purpose**: Create comprehensive unit tests for IUserCardsCommandAdapter interface following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Apis/IUserCardsCommandAdapterTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:13-30`
- Review command interface patterns from `src/Lib.Shared.Invocation.Tests/`
- Follow interface testing from existing adapter projects

**Testing Strategy**:
- Method signature validation for AddUserCardAsync
- Return type verification (IOperationResponse<IUserCardItrEntity>)
- Parameter type validation (IUserCardItrEntity)
- Interface contract verification

**Test Scenarios to Cover**:
- AddUserCardAsync method signature verification
- Return type matches IOperationResponse pattern
- Parameter accepts IUserCardItrEntity
- Async method pattern compliance

**Pattern References**:
- Command interface tests: Reference patterns from existing command interface tests
- IOperationResponse testing: Follow patterns from `src/Lib.Shared.Invocation.Tests/`
- Interface testing: Use established interface validation patterns

## User Story 3: Create UserCards Cosmos Item Entity

### Acceptance Criteria
- UserCardItem class defined in Cosmos adapter project
- Entity follows established Item patterns
- Properties match schema requirements

### Task 3.1: Create UserCardItem Class

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserCardItem.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/UserInfoItem.cs:1-19`

**Pattern to Follow**:
- Inherits from CosmosItem base class
- Override Id and Partition properties
- Use JsonProperty attributes for property naming
- Properties use init setters

**Modifications**:
- Class name: UserCardItem
- Id property returns CardId value
- Partition property returns UserId value
- Add properties with JsonProperty attributes:
  - UserId with json_property "user_id"
  - CardId with json_property "card_id"
  - SetId with json_property "set_id"
  - CollectedList with json_property "collected" (type: List<CollectedItem>)

### Task 3.2: Unit Tests for UserCardItem Class

**Parent Implementation Task**: Task 3.1 - Create UserCardItem Class

**Purpose**: Create comprehensive unit tests for UserCardItem class following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/CosmosItems/UserCardItemTests.cs`

**Reference Patterns**:
- Copy test structure from similar Cosmos item tests in `src/Lib.Adapter.Scryfall.Cosmos.Tests/` test projects
- Review JsonProperty testing patterns from existing item tests
- Follow CosmosItem inheritance testing patterns

**Testing Strategy**:
- Constructor and property initialization testing
- JsonProperty attribute verification
- Id and Partition property override testing
- Property getter/setter validation

**Test Scenarios to Cover**:
- Constructor creates instance with default values
- Id property returns CardId value correctly
- Partition property returns UserId value correctly
- JsonProperty attributes are properly configured
- Properties use init setters correctly
- CollectedList property handles List<CollectedItem> type

**Required Fakes**:
- No complex fakes needed - simple property testing

**Pattern References**:
- Cosmos item tests: Reference existing CosmosItem test patterns
- JsonProperty testing: Follow JSON serialization test examples
- Property testing: Use established property validation patterns

### Task 3.3: Create CollectedItem Nested Class

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/Nesteds/CollectedItem.cs`

**Reference**: Copy pattern from `src/Lib.Adapter.Scryfall.Cosmos/Apis/CosmosItems/Nesteds/RulingEntryItem.cs:1-16`

**Pattern to Follow**:
- Simple POCO class for nested data
- JsonProperty attributes for all properties
- Properties use init setters

**Modifications**:
- Create new class CollectedItem
- Add properties:
  - Finish with json_property "finish" (string type)
  - Special with json_property "special" (string type)
  - Count with json_property "count" (int type)

### Task 3.4: Unit Tests for CollectedItem Nested Class

**Parent Implementation Task**: Task 3.3 - Create CollectedItem Nested Class

**Purpose**: Create comprehensive unit tests for CollectedItem nested class following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/CosmosItems/Nesteds/CollectedItemTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Adapter.Scryfall.Cosmos.Tests/` nested item tests
- Review POCO class testing patterns from existing nested objects
- Follow JsonProperty testing from similar nested classes

**Testing Strategy**:
- Constructor and property testing
- JsonProperty attribute verification
- Property type validation
- Init setter functionality testing

**Test Scenarios to Cover**:
- Constructor creates instance with default values
- Finish property accepts string values with correct JsonProperty
- Special property accepts string values with correct JsonProperty
- Count property accepts int values with correct JsonProperty
- Properties use init setters correctly
- JsonProperty attributes have correct names

**Pattern References**:
- Nested class tests: Reference existing nested item test patterns
- POCO testing: Follow simple class testing patterns
- JsonProperty validation: Use JSON attribute testing examples

## User Story 4: Create UserCards Cosmos Container Infrastructure

### Acceptance Criteria
- Container definition created with proper partition key
- Container adapter created for Cosmos operations
- Container name primitive created

### Task 4.1: Create Container Name Primitive

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Primitives/UserCardsCosmosContainerName.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Primitives/UserInfoCosmosContainerName.cs:1-8`

**Pattern to Follow**:
- Inherits from CosmosContainerName
- Override AsSystemType method
- Internal sealed class

**Modifications**:
- Class name: UserCardsCosmosContainerName
- AsSystemType returns "UserCards"

### Task 4.2: Unit Tests for Container Name Primitive

**Parent Implementation Task**: Task 4.1 - Create Container Name Primitive

**Purpose**: Create comprehensive unit tests for UserCardsCosmosContainerName following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Cosmos/Primitives/UserCardsCosmosContainerNameTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Cosmos.Tests/Apis/Ids/CosmosContainerNameTests.cs:1-7`
- Review primitive testing patterns from Cosmos infrastructure tests
- Follow CosmosContainerName inheritance testing patterns

**Testing Strategy**:
- AsSystemType method return value verification
- Inheritance from CosmosContainerName validation
- String output format testing
- Class accessibility verification

**Test Scenarios to Cover**:
- AsSystemType_ReturnsUserCards_ReturnsCorrectString
- Constructor_CreatesInstance_WithoutException
- InheritsFromCosmosContainerName_CorrectBaseClass
- AsSystemType_ConsistentOutput_SameValueEachCall

**Pattern References**:
- Container name tests: `src/Lib.Cosmos.Tests/Apis/Ids/CosmosContainerNameTests.cs`
- Primitive testing: Follow existing primitive class test patterns
- Inheritance testing: Use base class verification patterns

### Task 4.3: Create Container Definition

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/Definitions/UserCardsCosmosContainerDefinition.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/Definitions/UserInfoCosmosContainerDefinition.cs`

**Pattern to Follow**:
- Implements ICosmosContainerDefinition
- Returns appropriate primitives for account, database, container, partition key

**Modifications**:
- Class name: UserCardsCosmosContainerDefinition
- ContainerName method returns new UserCardsCosmosContainerName()
- Keep same FriendlyAccountName (MtgDiscoveryCosmosAccountName)
- Keep same DatabaseName (MtgDiscoveryCosmosDatabaseName)
- Keep same PartitionKeyPath (PartitionCosmosPartitionKeyPath)

### Task 4.4: Unit Tests for Container Definition

**Parent Implementation Task**: Task 4.3 - Create Container Definition

**Purpose**: Create comprehensive unit tests for UserCardsCosmosContainerDefinition following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Cosmos/Containers/Definitions/UserCardsCosmosContainerDefinitionTests.cs`

**Reference Patterns**:
- Copy test structure from existing container definition tests in Cosmos projects
- Review ICosmosContainerDefinition testing patterns
- Follow container definition testing from infrastructure layer

**Testing Strategy**:
- Interface implementation verification
- Method return value validation
- Object instance creation testing
- Property consistency verification

**Test Scenarios to Cover**:
- ContainerName_ReturnsUserCardsCosmosContainerName_CorrectType
- FriendlyAccountName_ReturnsMtgDiscoveryCosmosAccountName_CorrectType
- DatabaseName_ReturnsMtgDiscoveryCosmosDatabaseName_CorrectType
- PartitionKeyPath_ReturnsPartitionCosmosPartitionKeyPath_CorrectType
- ImplementsICosmosContainerDefinition_CorrectInterface

**Pattern References**:
- Container definition tests: Reference existing definition test patterns
- Interface implementation testing: Follow established interface test patterns
- Method return testing: Use existing method validation patterns

### Task 4.5: Create Container Adapter

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/UserCardsCosmosContainer.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Cosmos/Containers/UserInfoCosmosContainer.cs:1-12`

**Pattern to Follow**:
- Inherits from CosmosContainerAdapter
- Internal sealed class
- Constructor accepts ILogger
- Passes definition and config to base

**Modifications**:
- Class name: UserCardsCosmosContainer
- Pass new UserCardsCosmosContainerDefinition() to base constructor
- Keep ServiceLocatorAuthCosmosConnectionConfig usage

### Task 4.6: Unit Tests for Container Adapter

**Parent Implementation Task**: Task 4.5 - Create Container Adapter

**Purpose**: Create comprehensive unit tests for UserCardsCosmosContainer following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Cosmos/Containers/UserCardsCosmosContainerTests.cs`

**Reference Patterns**:
- Copy test structure from existing container adapter tests
- Review CosmosContainerAdapter inheritance testing patterns
- Follow container testing from `src/Lib.Cosmos.Tests/`

**Testing Strategy**:
- Constructor dependency injection testing
- Inheritance from CosmosContainerAdapter verification
- Logger injection validation
- Base constructor parameter passing testing

**Test Scenarios to Cover**:
- Constructor_WithLogger_CreatesInstance
- Constructor_PassesDefinitionToBase_CorrectlyConfigured
- Constructor_PassesConfigToBase_CorrectlyConfigured
- InheritsFromCosmosContainerAdapter_CorrectBaseClass
- Constructor_WithNullLogger_ThrowsArgumentNullException

**Required Fakes**:
- LoggerFake for ILogger dependency

**Pattern References**:
- Container adapter tests: Reference existing adapter test patterns
- Constructor testing: Follow dependency injection test patterns
- Inheritance testing: Use base class verification examples

## User Story 5: Create UserCards Scribe Operator

### Acceptance Criteria
- Scribe operator created for UserCards write operations
- Inherits from CosmosScribe base class
- Properly integrated with container

### Task 5.1: Create UserCardsScribe Class

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserCardsScribe.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Scribes/UserInfoScribe.cs:1-12`

**Pattern to Follow**:
- Public sealed class
- Inherits from CosmosScribe
- Constructor accepts ILogger
- Passes container instance to base

**Modifications**:
- Class name: UserCardsScribe
- Pass new UserCardsCosmosContainer(logger) to base constructor

### Task 5.2: Unit Tests for UserCardsScribe Class

**Parent Implementation Task**: Task 5.1 - Create UserCardsScribe Class

**Purpose**: Create comprehensive unit tests for UserCardsScribe following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Apis/Operators/Scribes/UserCardsScribeTests.cs`

**Reference Patterns**:
- Copy test structure from existing scribe tests in Cosmos projects
- Review CosmosScribe inheritance testing patterns
- Follow scribe testing from `src/Lib.Adapter.Scryfall.Cosmos.Tests/` test projects

**Testing Strategy**:
- Constructor dependency injection testing
- Inheritance from CosmosScribe verification
- Logger injection and container passing validation
- Base constructor parameter testing

**Test Scenarios to Cover**:
- Constructor_WithLogger_CreatesInstance
- Constructor_PassesContainerToBase_CorrectlyConfigured
- InheritsFromCosmosScribe_CorrectBaseClass
- Constructor_WithNullLogger_ThrowsArgumentNullException
- Constructor_CreatesUserCardsCosmosContainer_CorrectlyInitialized

**Required Fakes**:
- LoggerFake for ILogger dependency

**Pattern References**:
- Scribe tests: Reference existing scribe test patterns from Cosmos projects
- Constructor testing: Follow dependency injection test patterns
- CosmosScribe inheritance: Use base class verification patterns

## User Story 6: Implement UserCards Command Adapter

### Acceptance Criteria
- Command adapter implementation created
- Properly handles entity mapping
- Returns appropriate operation responses

### Task 6.1: Create UserCardsCosmosCommandAdapter Class

**Location**: `src/Lib.Adapter.UserCards/Commands/UserCardsCosmosCommandAdapter.cs`

**Reference**: Copy from `src/Lib.Adapter.User/Persistence/UserCosmosPersistenceAdapter.cs:1-45`

**Pattern to Follow**:
- Internal sealed class implementing specialized interface
- Constructor injection of dependencies
- Private constructor for testing with explicit dependencies
- Async methods with ConfigureAwait(false)
- Entity mapping from ItrEntity to storage Item
- Returns IOperationResponse from Lib.Shared.Invocation

**Modifications**:
- Class name: UserCardsCosmosCommandAdapter
- Implements IUserCardsCommandAdapter
- Inject UserCardsScribe instead of UserInfoScribe
- Method name: AddUserCardAsync instead of RegisterUserAsync
- Parameter type: IUserCardItrEntity instead of IUserInfoItrEntity
- Map to UserCardItem instead of UserInfoItem
- Return type: IOperationResponse<IUserCardItrEntity>
- Map properties:
  - UserId from entity to item
  - CardId from entity to item
  - SetId from entity to item
  - Create empty CollectedList initially

### Task 6.2: Unit Tests for UserCardsCosmosCommandAdapter

**Parent Implementation Task**: Task 6.1 - Create UserCardsCosmosCommandAdapter Class

**Purpose**: Create comprehensive unit tests for UserCardsCosmosCommandAdapter following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Commands/UserCardsCosmosCommandAdapterTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:13-75`
- Review command adapter testing patterns from existing adapter tests
- Follow async testing patterns with ConfigureAwait(false)

**Testing Strategy**:
- TypeWrapper pattern for testing class with private constructor
- AddUserCardAsync method testing with various scenarios
- Entity mapping verification
- Scribe interaction validation
- IOperationResponse return type testing

**Test Scenarios to Cover**:
- AddUserCardAsync_ValidEntity_ReturnsSuccessResponse
- AddUserCardAsync_ValidEntity_CallsScribeUpsertOnce
- AddUserCardAsync_ValidEntity_MapsPropertiesCorrectly
- AddUserCardAsync_NullEntity_ThrowsArgumentNullException
- Constructor_WithLogger_CreatesInstance
- AddUserCardAsync_ScribeReturnsSuccess_ReturnsEntityResponse

**Required Fakes**:
- UserCardsScribeFake in `src/Lib.Adapter.UserCards.Tests/Fakes/UserCardsScribeFake.cs`
- FakeUserCardItrEntity in `src/Lib.Adapter.UserCards.Tests/Fakes/FakeUserCardItrEntity.cs`
- FakeOperationResponse<IUserCardItrEntity> from shared test infrastructure

**Pattern References**:
- Command adapter tests: Follow existing adapter test patterns
- TypeWrapper usage: `src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:15-18`
- Async testing: Use ConfigureAwait(false) patterns from existing tests
- Entity mapping verification: Follow property mapping test patterns

## User Story 7: Implement Main Adapter Service

### Acceptance Criteria
- Main adapter service delegates to specialized adapters
- Follows composite pattern
- Properly wired for dependency injection

### Task 7.1: Create UserCardsAdapterService Class

**Location**: `src/Lib.Adapter.UserCards/Apis/UserCardsAdapterService.cs`

**Reference**: Copy from `src/Lib.Adapter.User/Apis/UserAdapterService.cs`

**Pattern to Follow**:
- Public sealed class implementing main interface
- Constructor injection of ILogger
- Private constructor accepting specialized adapters
- Delegation pattern to specialized adapters
- Interface method implementations

**Modifications**:
- Class name: UserCardsAdapterService
- Implements IUserCardsAdapterService
- Field: _commandAdapter of type IUserCardsCommandAdapter
- Public constructor creates UserCardsCosmosCommandAdapter
- Private constructor accepts IUserCardsCommandAdapter
- Implement AddUserCardAsync by delegating to _commandAdapter

### Task 7.2: Unit Tests for UserCardsAdapterService

**Parent Implementation Task**: Task 7.1 - Create UserCardsAdapterService Class

**Purpose**: Create comprehensive unit tests for UserCardsAdapterService following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Apis/UserCardsAdapterServiceTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:13-75`
- Review composite service testing patterns from existing adapter services
- Follow delegation testing patterns from similar services

**Testing Strategy**:
- TypeWrapper pattern for testing class with private constructor
- Delegation verification to command adapter
- Constructor testing for both public and private constructors
- Interface implementation validation

**Test Scenarios to Cover**:
- Constructor_WithLogger_CreatesInstance
- AddUserCardAsync_CallsCommandAdapter_DelegatesToCorrectMethod
- AddUserCardAsync_ReturnsCommandAdapterResult_SameResponse
- Constructor_WithCommandAdapter_AcceptsInjectedDependency
- ImplementsIUserCardsAdapterService_CorrectInterface

**Required Fakes**:
- FakeUserCardsCommandAdapter in `src/Lib.Adapter.UserCards.Tests/Fakes/FakeUserCardsCommandAdapter.cs`
- LoggerFake from TestConvenience.Core
- FakeUserCardItrEntity for method parameter testing

**Pattern References**:
- Service delegation tests: `src/Lib.Aggregator.Cards.Tests/Apis/CardAggregatorServiceTests.cs:33-63`
- TypeWrapper pattern: Use for private constructor testing
- Composite service testing: Follow established delegation patterns
- Interface implementation: Verify proper interface contract fulfillment

## User Story 8: Create Custom Exception

### Acceptance Criteria
- Custom exception class created
- Extends OperationException from Lib.Shared.Invocation
- Follows established exception patterns

### Task 8.1: Create UserCardsAdapterException Class

**Location**: `src/Lib.Adapter.UserCards/Exceptions/UserCardsAdapterException.cs`

**Reference**: Copy from `src/Lib.Adapter.User/Exceptions/UserAdapterException.cs`

**Pattern to Follow**:
- Extends OperationException from Lib.Shared.Invocation.Exceptions
- Multiple constructors for different scenarios
- Passes messages to base class

**Modifications**:
- Class name: UserCardsAdapterException
- Update XML documentation
- Keep all constructor patterns

### Task 8.2: Unit Tests for UserCardsAdapterException

**Parent Implementation Task**: Task 8.1 - Create UserCardsAdapterException Class

**Purpose**: Create comprehensive unit tests for UserCardsAdapterException following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Exceptions/UserCardsAdapterExceptionTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Shared.Invocation.Tests/Exceptions/OperationExceptionTests.cs`
- Review exception testing patterns from existing exception tests
- Follow OperationException inheritance testing patterns

**Testing Strategy**:
- Constructor testing for all overloads
- Inheritance from OperationException verification
- Message passing validation
- Exception property testing

**Test Scenarios to Cover**:
- Constructor_WithMessage_SetsMessage
- Constructor_WithMessageAndInnerException_SetsProperties
- Constructor_Default_CreatesInstance
- InheritsFromOperationException_CorrectBaseClass
- Constructor_WithNullMessage_AcceptsNull
- Constructor_WithInnerException_PreservesInnerException

**Pattern References**:
- Exception tests: `src/Lib.Shared.Invocation.Tests/Exceptions/OperationExceptionTests.cs`
- Constructor testing: Follow exception constructor test patterns
- Inheritance testing: Verify base class inheritance correctly
- Exception property testing: Use established exception testing patterns

## User Story 9: Create IUserCardItrEntity Interface

### Acceptance Criteria
- Interface defined in Lib.Shared.DataModels
- Properties match schema requirements
- Follows ItrEntity naming convention

### Task 9.1: Define IUserCardItrEntity Interface

**Location**: `src/Lib.Shared.DataModels/Entities/IUserCardItrEntity.cs`

**Reference**: Copy from `src/Lib.Shared.DataModels/Entities/IUserInfoItrEntity.cs:1-8`

**Pattern to Follow**:
- Interface with string properties
- Properties use PascalCase
- No implementation, just interface definition

**Modifications**:
- Interface name: IUserCardItrEntity
- Properties to add:
  - string UserId { get; }
  - string CardId { get; }
  - string SetId { get; }
  - Initially skip collected list for tracer bullet

### Task 9.2: Unit Tests for IUserCardItrEntity Interface

**Parent Implementation Task**: Task 9.1 - Define IUserCardItrEntity Interface

**Purpose**: Create comprehensive unit tests for IUserCardItrEntity interface following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Entities/IUserCardItrEntityTests.cs`

**Reference Patterns**:
- Copy test structure from existing entity interface tests in shared projects
- Review ItrEntity interface testing patterns
- Follow interface property testing from `src/Lib.Shared.DataModels/` tests

**Testing Strategy**:
- Interface property signature verification
- Property accessibility testing
- Interface contract validation
- Property type verification

**Test Scenarios to Cover**:
- UserId_Property_HasStringType
- CardId_Property_HasStringType
- SetId_Property_HasStringType
- Properties_AreReadOnly_GetterOnly
- Interface_IsPublic_CorrectAccessibility
- Properties_FollowItrEntityConvention_NamingPattern

**Pattern References**:
- Entity interface tests: Reference existing ItrEntity interface test patterns
- Interface testing: Follow established interface validation patterns
- Property testing: Use property signature verification examples
- ItrEntity conventions: Follow naming and structure patterns from existing entities

## User Story 10: Create Unit Tests for Adapter

### Acceptance Criteria
- Test project created with proper references
- Tests follow TESTING_GUIDELINES.md patterns
- All public methods have tests
- Fake implementations created

### Task 10.1: Create Test Project

**Location**: `src/Lib.Adapter.UserCards.Tests/`

**Reference**: Copy structure from `src/Lib.Aggregator.Cards.Tests/`

**Pattern to Follow**:
- MSTest framework
- Reference to main project
- Reference to TestConvenience.Core
- InternalsVisibleTo configured in main project

**Modifications**:
- Project name: Lib.Adapter.UserCards.Tests
- Update namespace to match

### Task 10.2: Unit Tests for Test Project Configuration

**Parent Implementation Task**: Task 10.1 - Create Test Project

**Purpose**: Verify test project configuration and build following TESTING_GUIDELINES.md patterns.

**Test File Location**: Build verification task - validated through successful test execution

**Testing Strategy**:
- Test project builds successfully
- All project references resolve correctly
- MSTest framework properly configured
- TestConvenience.Core integration working

**Validation Approach**:
- Successful dotnet build of test project
- Test discovery and execution verification
- Project reference accessibility validation
- InternalsVisibleTo configuration verification

**Reference Patterns**:
- Test project configuration: `src/Lib.Aggregator.Cards.Tests/`
- MSTest framework setup: Follow existing test project patterns
- Project reference patterns: Use established test project reference structure

### Task 10.3: Create Fake Scribe

**Location**: `src/Lib.Adapter.UserCards.Tests/Fakes/UserCardsScribeFake.cs`

**Reference**: Pattern from test convenience documentation

**Pattern to Follow**:
- Implements scribe interface or inherits base
- Tracks invocation counts
- Returns configured responses
- Properties for verification

**Implementation Details**:
- Create fake that tracks UpsertAsync calls
- Property for UpsertAsyncInvocationCount
- Property for LastUpsertedItem
- Override UpsertAsync to increment counter and store item

### Task 10.4: Unit Tests for Fake Scribe

**Parent Implementation Task**: Task 10.3 - Create Fake Scribe

**Purpose**: Create unit tests for UserCardsScribeFake following TESTING_GUIDELINES.md patterns.

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Fakes/UserCardsScribeFakeTests.cs`

**Reference Patterns**:
- Copy test structure from existing fake tests in test projects
- Review fake implementation testing patterns
- Follow fake verification patterns from `src/Lib.Aggregator.Cards.Tests/Fakes/`

**Testing Strategy**:
- Fake method invocation tracking verification
- Property initialization testing
- Fake behavior validation
- Invocation count accuracy testing

**Test Scenarios to Cover**:
- UpsertAsync_Called_IncrementsInvocationCount
- UpsertAsync_WithItem_StoresLastUpsertedItem
- UpsertAsync_MultipleCalls_CountsCorrectly
- UpsertAsync_ReturnsConfiguredResult_WhenSet
- Constructor_InitializesProperties_DefaultValues

**Pattern References**:
- Fake testing: Follow existing fake validation patterns
- Invocation tracking: Use established invocation count testing
- Fake behavior: Verify fake acts as expected substitute

### Task 10.5: Create Adapter Tests

**Location**: `src/Lib.Adapter.UserCards.Tests/Commands/UserCardsCosmosCommandAdapterTests.cs`

**Reference**: Test patterns from TESTING_GUIDELINES.md

**Pattern to Follow**:
- Self-contained tests
- Arrange-Act-Assert pattern
- Test naming: MethodName_Scenario_ExpectedBehavior
- Return value named 'actual'
- Verify fake invocation counts

**Test Cases**:
- AddUserCardAsync_ValidEntity_ReturnsSuccessResponse
- AddUserCardAsync_ValidEntity_CallsScribeOnce
- AddUserCardAsync_ValidEntity_MapsPropertiesCorrectly
- AddUserCardAsync_NullEntity_ThrowsException

### Task 10.6: Unit Tests for Adapter Test Implementation

**Parent Implementation Task**: Task 10.5 - Create Adapter Tests

**Purpose**: Ensure adapter tests are comprehensive and follow TESTING_GUIDELINES.md patterns.

**Test File Location**: Validation of test implementation quality and coverage

**Testing Strategy**:
- Verify all test scenarios are covered
- Confirm test naming follows conventions
- Validate Arrange-Act-Assert structure
- Ensure proper fake usage and verification

**Test Quality Validation**:
- All AddUserCardAsync scenarios covered
- TypeWrapper pattern used correctly
- ConfigureAwait(false) used on async calls
- Fake invocation counts verified
- Exception testing properly implemented

**Pattern References**:
- Test quality standards: TESTING_GUIDELINES.md
- Test coverage patterns: Follow comprehensive testing examples
- Test structure validation: Ensure proper test organization

### Task 10.7: Create Service Tests

**Location**: `src/Lib.Adapter.UserCards.Tests/Apis/UserCardsAdapterServiceTests.cs`

**Reference**: Test patterns from TESTING_GUIDELINES.md

**Pattern to Follow**:
- Test delegation to command adapter
- Use fake command adapter
- Verify delegation occurs correctly

**Test Cases**:
- AddUserCardAsync_CallsCommandAdapter
- Constructor_CreatesCommandAdapter

### Task 10.8: Unit Tests for Service Test Implementation

**Parent Implementation Task**: Task 10.7 - Create Service Tests

**Purpose**: Ensure service tests properly validate delegation and follow TESTING_GUIDELINES.md patterns.

**Test File Location**: Validation of service test implementation quality

**Testing Strategy**:
- Verify delegation testing is comprehensive
- Confirm constructor testing covers all scenarios
- Validate fake usage and verification patterns
- Ensure proper test structure and naming

**Test Quality Validation**:
- Delegation to command adapter verified
- Constructor testing covers public and private constructors
- TypeWrapper pattern used for private constructor testing
- Fake command adapter properly configured and verified
- Interface implementation testing included

**Pattern References**:
- Service delegation testing: Follow established delegation patterns
- Constructor testing: Use comprehensive constructor validation
- Fake verification: Ensure proper fake interaction testing

## User Story 11: Integration and Build Verification

### Acceptance Criteria
- Project added to solution file
- All projects build successfully
- No compiler warnings
- Tests pass

### Task 11.1: Add Project to Solution

**Location**: `src/MtgDiscoveryVibe.sln`

**Steps**:
1. Add Lib.Adapter.UserCards project to solution
2. Add Lib.Adapter.UserCards.Tests project to solution
3. Place in appropriate solution folders

**Commands**:
- Use dotnet sln add command for both projects
- Verify with dotnet build

### Task 11.2: Unit Tests for Solution Integration

**Parent Implementation Task**: Task 11.1 - Add Project to Solution

**Purpose**: Verify solution integration and project references following TESTING_GUIDELINES.md patterns.

**Test File Location**: Build and integration verification task

**Testing Strategy**:
- Solution builds successfully with new projects
- Project references resolve correctly across solution
- No compilation warnings or errors
- Projects are properly organized in solution structure

**Validation Approach**:
- Execute dotnet build on entire solution
- Verify no build errors or warnings
- Confirm project discovery in solution
- Validate solution structure organization

**Reference Patterns**:
- Solution integration: Follow existing project addition patterns
- Build verification: Use established build validation practices
- Project organization: Match existing solution folder structure

### Task 11.3: Build and Test

**Steps**:
1. Build entire solution
2. Run all tests
3. Fix any compilation errors
4. Resolve any warnings

**Commands**:
- dotnet build src/MtgDiscoveryVibe.sln
- dotnet test src/Lib.Adapter.UserCards.Tests/

### Task 11.4: Unit Tests for Build and Test Verification

**Parent Implementation Task**: Task 11.3 - Build and Test

**Purpose**: Comprehensive validation of build success and test execution following TESTING_GUIDELINES.md patterns.

**Test File Location**: Build and test execution verification task

**Testing Strategy**:
- All projects build without errors
- All unit tests pass successfully
- No compiler warnings present
- Test coverage meets standards

**Validation Approach**:
- Execute full solution build verification
- Run all UserCards adapter tests
- Verify test discovery and execution
- Confirm zero build warnings
- Validate test results and coverage

**Reference Patterns**:
- Build verification: Follow established build validation practices
- Test execution: Use comprehensive test running patterns
- Quality assurance: Match existing quality standards

## Validation Checklist

### Project Structure
- [ ] Lib.Adapter.UserCards project created
- [ ] Folder structure matches pattern (Apis, Commands, Exceptions, Entities)
- [ ] Project references configured correctly
- [ ] InternalsVisibleTo for test project

### Cosmos Integration
- [ ] UserCardItem class in Lib.Adapter.Scryfall.Cosmos
- [ ] CollectedItem nested class created
- [ ] UserCardsScribe operator created
- [ ] Container infrastructure (name, definition, adapter)

### Adapter Implementation
- [ ] IUserCardsAdapterService interface
- [ ] IUserCardsCommandAdapter interface
- [ ] UserCardsCosmosCommandAdapter implementation
- [ ] UserCardsAdapterService implementation
- [ ] UserCardsAdapterException

### Shared Entities
- [ ] IUserCardItrEntity interface in Lib.Shared.DataModels

### Testing
- [ ] Test project created
- [ ] Fake implementations
- [ ] Unit tests for command adapter
- [ ] Unit tests for service
- [ ] All tests pass

### Build Verification
- [ ] Solution builds without errors
- [ ] No compiler warnings
- [ ] All tests pass

## Next Steps (Future User Stories)

1. **Query Operations**: Add Gopher/Inquisitor for reading user cards
2. **Collected List Management**: Add/update/remove items from collected array
3. **Bulk Operations**: Batch add/update for multiple cards
4. **Cache Integration**: Add caching layer for user collections
5. **Metrics**: Add telemetry and monitoring

## Notes

### Design Decisions
- Using CQRS pattern with Commands folder (not Persistence like User adapter)
- Tracer bullet approach: basic write operation first, enhance later
- Following established patterns from existing adapters
- IOperationResponse from Lib.Shared.Invocation (NOT OpResponse from Lib.Cosmos)

### Key Patterns to Follow
- MicroObjects: Preserve ItrEntity until external system boundary
- No nulls: Use Null Object pattern
- Private readonly fields
- Constructor injection only
- ConfigureAwait(false) on all async calls
- Internal implementations, public interfaces in Apis folder

### Common Pitfalls to Avoid
- Don't use OpResponse from Lib.Cosmos - use IOperationResponse from Lib.Shared.Invocation
- Don't extract primitives at layer boundaries - pass complete ItrEntity objects
- Don't forget ConfigureAwait(false) on async calls
- Don't make internal classes public
- Don't forget to add InternalsVisibleTo for test project