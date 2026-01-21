# UserCards by Set Query Implementation Story

## Overview
Implementation of a GraphQL query that returns all user cards for a specific collector within a given set. This query enables users to see which cards they own from a particular set, supporting collection management by set.

## Context
The UserCard data is stored in Cosmos DB with:
- Partition Key: UserId (collector_id)
- Document Id: CardId
- Non-id field: SetId (set_id)

The query needs to:
- Accept collector_id and set_id as parameters
- Query Cosmos using the collector_id as partition key
- Filter results by set_id field
- Return all matching UserCard collection data

## Reference Projects
- **Pattern Reference**: `src/Lib.Adapter.Sets/` - Shows query adapter patterns with multiple parameters
- **Test Reference**: `src/Lib.Adapter.Sets.Tests/` - Test patterns for query adapters
- **UserCards Reference**: `src/Lib.Adapter.UserCards/` - Existing UserCards command adapter patterns
- **Query Pattern**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/` - Query definition patterns
- **GraphQL Pattern**: `src/App.MtgDiscovery.GraphQL/Queries/SetQueryMethods.cs` - GraphQL query method patterns

## User Story: UserCards by Set Query Complete Implementation

### Description
Complete implementation of UserCards by Set query including all components from Adapter layer through GraphQL, comprehensive tests, pull request, and approval.

### Acceptance Criteria
- All implementation tasks completed
- All test tasks completed and passing
- Pull request created and passing all checks
- User approval obtained

### TSK-IMPL 1: Create UserCards by Set Inquisition Query Definition

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/SelectUserCardsBySetQueryDefinition.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/SelectAllSetItemsQueryDefinition.cs`

**Steps**:
1. Copy the reference file to the new location
2. Update class name to SelectUserCardsBySetQueryDefinition
3. Modify the query definition to filter by set_id

**Modifications** (describe changes, do not show code):
- Change class name from SelectAllSetItemsQueryDefinition to SelectUserCardsBySetQueryDefinition
- Update namespace if needed
- Change the QueryDefinition from "SELECT * FROM c" to parameterized query
- Query should be: "SELECT * FROM c WHERE c.set_id = @setId"
- This creates a parameterized query that filters UserCardItems by their set_id field

**Pattern to Follow** (reference only, no code):
- Inherit from InquiryDefinition abstract class
- Override AsSystemType() method to return QueryDefinition
- Use parameterized query with @setId parameter for safe querying
- Keep the class sealed and follow the simple pattern from reference

### TSK-TEST 1: Unit Tests for SelectUserCardsBySetQueryDefinition

**Test File Location**: `src/Lib.Adapter.Scryfall.Cosmos.Tests/Apis/Operators/Inquisitions/SelectUserCardsBySetQueryDefinitionTests.cs`

**Reference Patterns**:
- Create simple test class following MSTest pattern
- Test that AsSystemType() returns non-null QueryDefinition
- Test that the query text contains expected parameter

**Test Scenarios to Cover**:
- Constructor creates valid instance
- AsSystemType() returns QueryDefinition
- Query contains @setId parameter
- Query selects from correct source

**Pattern to Follow**:
- Use [TestClass] and [TestMethod] attributes
- Follow MethodName_Scenario_ExpectedBehavior naming
- Use AwesomeAssertions for fluent assertions
- Keep tests self-contained

### TSK-IMPL 2: Create UserCards by Set Inquisition

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/SelectUserCardsBySetInquisition.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/SelectAllSetItemsInquisition.cs`

**Steps**:
1. Copy the reference file to the new location
2. Update class name to SelectUserCardsBySetInquisition
3. Update to use UserCardsInquisitor instead of ScryfallSetItemsInquisitor
4. Update to use SelectUserCardsBySetQueryDefinition
5. Add parameter support for set_id

**Modifications** (describe changes, do not show code):
- Change class name to SelectUserCardsBySetInquisition
- Add string setId parameter to constructor
- Store setId as private readonly field
- Pass UserCardsInquisitor (new type we'll create) to base
- Pass SelectUserCardsBySetQueryDefinition to base
- In QueryAsync method, add query parameters using WithParameter on the inquiry
- Use provided partition key (will be userId) instead of PartitionKey.None

**Pattern to Follow** (reference only, no code):
- Implement ICosmosInquisition interface
- Accept setId in constructor
- Use dependency injection pattern for inquisitor and query definition
- Add query parameter binding in QueryAsync
- Pass partition key through to inquisitor

### TSK-TEST 2: Unit Tests for SelectUserCardsBySetInquisition

**Test File Location**: `src/Lib.Adapter.Scryfall.Cosmos.Tests/Apis/Operators/Inquisitions/SelectUserCardsBySetInquisitionTests.cs`

**Reference Patterns**:
- Copy test structure from similar inquisition tests
- Use fakes for ICosmosInquisitor dependency
- Verify query parameter passing

**Test Scenarios to Cover**:
- Constructor validation with null/empty setId
- QueryAsync delegates to inquisitor with correct parameters
- Query parameters are properly bound
- Partition key is passed through correctly

**Required Fakes**:
- ICosmosInquisitor fake with invocation tracking

### TSK-IMPL 3: Create UserCards Inquisitor

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/UserCardsInquisitor.cs`

**Reference**: Copy from `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitors/ScryfallSetItemsInquisitor.cs`

**Steps**:
1. Copy the reference file to the new location
2. Update class name to UserCardsInquisitor
3. Change container to UserCardsCosmosContainer

**Modifications** (describe changes, do not show code):
- Change class name from ScryfallSetItemsInquisitor to UserCardsInquisitor
- Update constructor to use UserCardsCosmosContainer instead of ScryfallSetItemsCosmosContainer
- Keep the same pattern of passing logger to container constructor

**Pattern to Follow** (reference only, no code):
- Inherit from CosmosInquisitor base class
- Pass container to base constructor
- Accept ILogger in constructor
- Keep class sealed and internal visibility

### TSK-TEST 3: Unit Tests for UserCardsInquisitor

**Test File Location**: `src/Lib.Adapter.Scryfall.Cosmos.Tests/Apis/Operators/Inquisitors/UserCardsInquisitorTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Adapter.Scryfall.Cosmos.Tests/Apis/Operators/Inquisitors/ScryfallSetItemsInquisitorTests.cs`
- Test constructor and inheritance

**Test Scenarios to Cover**:
- Constructor creates valid instance
- Inherits from CosmosInquisitor
- Container is properly initialized

### TSK-IMPL 4: Create UserCards Query Adapter Interface

**Location**: `src/Lib.Adapter.UserCards/Apis/IUserCardsQueryAdapter.cs`

**Reference**: Copy from `src/Lib.Adapter.UserCards/Apis/IUserCardsCommandAdapter.cs`

**Steps**:
1. Copy the command adapter interface as reference
2. Create new interface IUserCardsQueryAdapter
3. Define method for GetUserCardsBySetAsync

**Modifications** (describe changes, do not show code):
- Create new interface IUserCardsQueryAdapter
- Add method: GetUserCardsBySetAsync
- Method parameters: string collectorId, string setId
- Return type: Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>>
- Add XML documentation for the method

**Pattern to Follow** (reference only, no code):
- Interface should be public
- Use async Task pattern
- Return IOperationResponse wrapper
- Accept primitive parameters (strings)
- Include comprehensive XML documentation

### TSK-TEST 4: Unit Tests for IUserCardsQueryAdapter Interface

**Note**: Interfaces don't need direct tests, but will be tested through implementation

### TSK-IMPL 5: Implement UserCards Query Adapter

**Location**: `src/Lib.Adapter.UserCards/Queries/UserCardsQueryAdapter.cs`

**Reference**: Copy from `src/Lib.Adapter.Sets/Queries/SetCosmosQueryAdapter.cs`

**Steps**:
1. Copy the reference file structure
2. Implement IUserCardsQueryAdapter interface
3. Add GetUserCardsBySetAsync implementation

**Modifications** (describe changes, do not show code):
- Change class name to UserCardsQueryAdapter
- Implement IUserCardsQueryAdapter interface
- Add dependencies: UserCardsGopher, SelectUserCardsBySetInquisition, mapper
- In GetUserCardsBySetAsync method:
  - Create SelectUserCardsBySetInquisition with setId parameter
  - Create PartitionKeyValue from collectorId
  - Call inquisition.QueryAsync with partition key
  - Map results from UserCardItem to IUserCardCollectionItrEntity
  - Return SuccessOperationResponse with mapped results
- Handle failure cases with UserCardsAdapterException

**Pattern to Follow** (reference only, no code):
- Internal sealed class pattern
- Constructor dependency injection
- Private constructor with interface parameters
- Public constructor creating concrete instances
- ConfigureAwait(false) on all async calls
- Return IOperationResponse<T> from Lib.Shared.Invocation

### TSK-TEST 5: Unit Tests for UserCardsQueryAdapter

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Queries/UserCardsQueryAdapterTests.cs`

**Reference Patterns**:
- Copy test structure from `src/Lib.Adapter.Sets.Tests/Queries/SetCosmosQueryAdapterTests.cs`
- Use TypeWrapper pattern for testing internal class

**Test Scenarios to Cover**:
- Constructor validation
- GetUserCardsBySetAsync with valid parameters returns success
- GetUserCardsBySetAsync with no results returns empty collection
- GetUserCardsBySetAsync with query failure returns failure response
- Verify partition key is created from collectorId
- Verify setId is passed to inquisition

**Required Fakes**:
- ICosmosInquisition fake
- Mapper fake
- Logger fake

### TSK-IMPL 6: Update UserCards Adapter Service Interface

**Location**: `src/Lib.Adapter.UserCards/Apis/IUserCardsAdapterService.cs`

**Reference**: Check existing file

**Steps**:
1. Add inheritance from IUserCardsQueryAdapter interface
2. Ensure interface composition pattern is followed

**Modifications** (describe changes, do not show code):
- Add IUserCardsQueryAdapter to the inheritance list
- Interface should now inherit from both IUserCardsCommandAdapter and IUserCardsQueryAdapter
- Follow the composite interface pattern seen in other adapter services

**Pattern to Follow** (reference only, no code):
- Composite interface pattern
- Interface inherits from specialized interfaces
- No method declarations in composite interface

### TSK-TEST 6: Unit Tests for Interface Updates

**Note**: Interface inheritance doesn't need direct tests

### TSK-IMPL 7: Update UserCards Adapter Service Implementation

**Location**: `src/Lib.Adapter.UserCards/Apis/UserCardsAdapterService.cs`

**Reference**: Check existing file and `src/Lib.Adapter.Sets/Apis/SetAdapterService.cs`

**Steps**:
1. Add query adapter as dependency
2. Implement IUserCardsQueryAdapter methods through delegation

**Modifications** (describe changes, do not show code):
- Add private readonly IUserCardsQueryAdapter field
- Update constructor to create/accept UserCardsQueryAdapter
- Implement GetUserCardsBySetAsync by delegating to query adapter
- Follow delegation pattern from reference

**Pattern to Follow** (reference only, no code):
- Service delegates to specialized adapters
- Constructor creates concrete instances
- Methods simply delegate with ConfigureAwait(false)
- Internal sealed class pattern

### TSK-TEST 7: Unit Tests for UserCardsAdapterService Updates

**Test File Location**: `src/Lib.Adapter.UserCards.Tests/Apis/UserCardsAdapterServiceTests.cs`

**Reference Patterns**:
- Update existing tests or add new test methods
- Verify delegation to query adapter

**Test Scenarios to Cover**:
- GetUserCardsBySetAsync delegates to query adapter
- Parameters are passed correctly
- Response is returned without modification

### TSK-IMPL 8: Update UserCards Aggregator Interface

**Location**: `src/Lib.Aggregator.UserCards/Apis/IUserCardsAggregatorService.cs`

**Reference**: Check existing file

**Steps**:
1. Add GetUserCardsBySetAsync method signature

**Modifications** (describe changes, do not show code):
- Add method: GetUserCardsBySetAsync
- Parameters: string collectorId, string setId
- Return type: Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>>
- Add XML documentation

**Pattern to Follow** (reference only, no code):
- Match the adapter interface signature
- Keep parameters as primitives at this layer
- Return IOperationResponse wrapper

### TSK-TEST 8: Unit Tests for Aggregator Interface

**Note**: Interfaces don't need direct tests

### TSK-IMPL 9: Implement UserCards Aggregator Method

**Location**: `src/Lib.Aggregator.UserCards/Apis/UserCardsAggregatorService.cs`

**Reference**: Check existing file and pattern

**Steps**:
1. Add GetUserCardsBySetAsync implementation
2. Delegate to adapter service

**Modifications** (describe changes, do not show code):
- Implement GetUserCardsBySetAsync method
- Simply delegate to _userCardsAdapterService.GetUserCardsBySetAsync
- Pass through collectorId and setId parameters
- Use ConfigureAwait(false) on async call
- Return the adapter response directly (no additional aggregation needed for single source)

**Pattern to Follow** (reference only, no code):
- Aggregator coordinates multiple adapters if needed
- For single adapter, simple delegation
- No business logic at this layer
- ConfigureAwait(false) on all async calls

### TSK-TEST 9: Unit Tests for UserCardsAggregatorService

**Test File Location**: `src/Lib.Aggregator.UserCards.Tests/Apis/UserCardsAggregatorServiceTests.cs`

**Reference Patterns**:
- Add test methods to existing test class
- Verify delegation pattern

**Test Scenarios to Cover**:
- GetUserCardsBySetAsync delegates to adapter
- Parameters passed correctly
- Response returned without modification
- ConfigureAwait(false) is used

**Required Fakes**:
- IUserCardsAdapterService fake

### TSK-IMPL 10: Update UserCards Domain Interface

**Location**: `src/Lib.Domain.UserCards/Apis/IUserCardsDomainService.cs`

**Reference**: Check existing file

**Steps**:
1. Add GetUserCardsBySetAsync method signature

**Modifications** (describe changes, do not show code):
- Add method: GetUserCardsBySetAsync
- Parameters: string collectorId, string setId
- Return type: Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>>
- Add XML documentation

**Pattern to Follow** (reference only, no code):
- Domain layer applies business rules
- Keep signature consistent with aggregator
- Return IOperationResponse wrapper

### TSK-TEST 10: Unit Tests for Domain Interface

**Note**: Interfaces don't need direct tests

### TSK-IMPL 11: Implement UserCards Domain Method

**Location**: `src/Lib.Domain.UserCards/Apis/UserCardsDomainService.cs`

**Reference**: Check existing file

**Steps**:
1. Add GetUserCardsBySetAsync implementation
2. Apply any domain rules if needed
3. Delegate to aggregator

**Modifications** (describe changes, do not show code):
- Implement GetUserCardsBySetAsync method
- For now, simply delegate to aggregator (no domain rules identified)
- Pass through parameters to _userCardsAggregatorService.GetUserCardsBySetAsync
- Use ConfigureAwait(false)
- Return aggregator response

**Pattern to Follow** (reference only, no code):
- Domain applies "ALWAYS" business rules
- If no rules, simple delegation
- ConfigureAwait(false) on async calls
- Could add filtering/validation if needed

### TSK-TEST 11: Unit Tests for UserCardsDomainService

**Test File Location**: `src/Lib.Domain.UserCards.Tests/Apis/UserCardsDomainServiceTests.cs`

**Reference Patterns**:
- Add test methods to existing test class
- Verify delegation

**Test Scenarios to Cover**:
- GetUserCardsBySetAsync delegates to aggregator
- Parameters passed correctly
- Response returned without modification

**Required Fakes**:
- IUserCardsAggregatorService fake

### TSK-IMPL 12: Create Entry Layer Query Interface

**Location**: `src/Lib.MtgDiscovery.Entry/Apis/IUserCardsQueryEntryService.cs`

**Reference**: Copy pattern from `src/Lib.MtgDiscovery.Entry/Apis/IUserCardsEntryService.cs`

**Steps**:
1. Create new interface for query operations
2. Add GetUserCardsBySetAsync method

**Modifications** (describe changes, do not show code):
- Create interface IUserCardsQueryEntryService
- Add method: GetUserCardsBySetAsync
- Parameters: IAuthUserArgEntity authUser, IUserCardsSetArgEntity setArgs
- Return: Task<IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>>>
- This separates query from command operations

**Pattern to Follow** (reference only, no code):
- Entry layer accepts ArgEntity parameters
- Includes auth user for security
- Returns IOperationResponse
- Separate interfaces for commands and queries

### TSK-TEST 12: Unit Tests for Entry Interface

**Note**: Interfaces don't need direct tests

### TSK-IMPL 13: Create UserCardsSetArgEntity

**Location**: `src/App.MtgDiscovery.GraphQL/Entities/Args/UserCards/UserCardsSetArgEntity.cs`

**Reference**: Copy from `src/App.MtgDiscovery.GraphQL/Entities/Args/SetIdsArgEntity.cs`

**Steps**:
1. Create new argument entity class
2. Add SetId property

**Modifications** (describe changes, do not show code):
- Create class UserCardsSetArgEntity
- Implement IUserCardsSetArgEntity interface (will create next)
- Add property: string SetId { get; init; }
- Add GraphQL attributes for the property
- Follow DTO pattern with init setters

**Pattern to Follow** (reference only, no code):
- Public class in Args folder
- Implement corresponding interface
- Use init setters for properties
- Add GraphQL attributes

### TSK-TEST 13: Unit Tests for UserCardsSetArgEntity

**Test File Location**: `src/App.MtgDiscovery.GraphQL.Tests/Entities/Args/UserCards/UserCardsSetArgEntityTests.cs`

**Test Scenarios to Cover**:
- Property can be set and retrieved
- Implements correct interface

### TSK-IMPL 14: Create IUserCardsSetArgEntity Interface

**Location**: `src/Lib.Shared.DataModels/Entities/IUserCardsSetArgEntity.cs`

**Reference**: Copy from `src/Lib.Shared.DataModels/Entities/ISetIdsItrEntity.cs`

**Steps**:
1. Create interface with SetId property

**Modifications** (describe changes, do not show code):
- Create interface IUserCardsSetArgEntity
- Add property: string SetId { get; }
- Add XML documentation

**Pattern to Follow** (reference only, no code):
- Interface in Shared.DataModels
- Read-only property
- Clear documentation

### TSK-TEST 14: Unit Tests for Interface

**Note**: Interfaces don't need direct tests

### TSK-IMPL 15: Implement Entry Layer Query Service

**Location**: `src/Lib.MtgDiscovery.Entry/Queries/UserCardsQueryEntryService.cs`

**Reference**: Copy from `src/Lib.MtgDiscovery.Entry/Queries/SetEntryService.cs`

**Steps**:
1. Create implementation of IUserCardsQueryEntryService
2. Add validation and mapping
3. Delegate to domain service

**Modifications** (describe changes, do not show code):
- Create class UserCardsQueryEntryService
- Implement IUserCardsQueryEntryService
- Add dependencies: domain service, validator, mapper
- In GetUserCardsBySetAsync:
  - Validate the setArgs
  - Extract userId from authUser
  - Call domain service with userId and setId
  - Return response

**Pattern to Follow** (reference only, no code):
- Internal sealed class
- Constructor dependency injection
- Validation before processing
- Extract primitives from entities
- Delegate to domain layer

### TSK-TEST 15: Unit Tests for UserCardsQueryEntryService

**Test File Location**: `src/Lib.MtgDiscovery.Entry.Tests/Queries/UserCardsQueryEntryServiceTests.cs`

**Reference Patterns**:
- Copy from `src/Lib.MtgDiscovery.Entry.Tests/Queries/SetEntryServiceTests.cs`
- Use TypeWrapper for internal class

**Test Scenarios to Cover**:
- Constructor validation
- GetUserCardsBySetAsync with valid args succeeds
- GetUserCardsBySetAsync with invalid args returns validation failure
- UserId extracted from auth user
- SetId extracted from args
- Delegation to domain service

**Required Fakes**:
- IUserCardsDomainService fake
- Validator fake
- AuthUser fake

### TSK-IMPL 16: Update Main Entry Service Interface

**Location**: `src/Lib.MtgDiscovery.Entry/Apis/IEntryService.cs`

**Reference**: Check existing file

**Steps**:
1. Add inheritance from IUserCardsQueryEntryService

**Modifications** (describe changes, do not show code):
- Add IUserCardsQueryEntryService to inheritance list
- Maintains composite interface pattern

**Pattern to Follow** (reference only, no code):
- Composite interface inherits from all specialized interfaces
- No method declarations in composite

### TSK-TEST 16: Unit Tests for Interface Updates

**Note**: Interface inheritance doesn't need direct tests

### TSK-IMPL 17: Update Main Entry Service Implementation

**Location**: `src/Lib.MtgDiscovery.Entry/EntryService.cs`

**Reference**: Check existing file

**Steps**:
1. Add query entry service as dependency
2. Delegate query method

**Modifications** (describe changes, do not show code):
- Add private field for IUserCardsQueryEntryService
- Update constructor to create UserCardsQueryEntryService
- Implement GetUserCardsBySetAsync by delegating
- Use ConfigureAwait(false)

**Pattern to Follow** (reference only, no code):
- Main service delegates to specialized services
- Constructor creates concrete instances
- Simple delegation with ConfigureAwait(false)

### TSK-TEST 17: Unit Tests for EntryService Updates

**Test File Location**: Update existing `src/Lib.MtgDiscovery.Entry.Tests/EntryServiceTests.cs`

**Test Scenarios to Cover**:
- GetUserCardsBySetAsync delegates correctly
- Parameters passed through
- Response returned unchanged

### TSK-IMPL 18: Create GraphQL Query Methods

**Location**: `src/App.MtgDiscovery.GraphQL/Queries/UserCardsQueryMethods.cs`

**Reference**: Copy from `src/App.MtgDiscovery.GraphQL/Queries/SetQueryMethods.cs`

**Steps**:
1. Create new query methods class
2. Add UserCardsBySet query method
3. Use authorization attribute

**Modifications** (describe changes, do not show code):
- Create class UserCardsQueryMethods
- Add [ExtendObjectType(typeof(ApiQuery))] attribute
- Add UserCardsBySet method with [Authorize] attribute
- Accept ClaimsPrincipal and UserCardsSetArgEntity parameters
- Create AuthUserArgEntity from claims
- Call entry service GetUserCardsBySetAsync
- Map response to GraphQL response model
- Return appropriate success/failure response

**Pattern to Follow** (reference only, no code):
- Extend ApiQuery with ExtendObjectType
- Use [Authorize] for user-specific queries
- Extract user from ClaimsPrincipal
- Return union type responses
- Map ITR entities to OUT entities

### TSK-TEST 18: Unit Tests for UserCardsQueryMethods

**Test File Location**: `src/App.MtgDiscovery.GraphQL.Tests/Queries/UserCardsQueryMethodsTests.cs`

**Reference Patterns**:
- Copy from mutation tests with ClaimsPrincipal
- Test authorization and response mapping

**Test Scenarios to Cover**:
- Constructor creates instance
- UserCardsBySet with valid auth returns success
- UserCardsBySet with service failure returns failure response
- ClaimsPrincipal properly extracted
- Response mapping works correctly

**Required Fakes**:
- IEntryService fake
- ClaimsPrincipal setup
- Mapper fake

### TSK-IMPL 19: Create Response Type for User Cards Collection

**Location**: `src/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/UserCardsCollectionResponseModelUnionType.cs`

**Reference**: Copy from `src/App.MtgDiscovery.GraphQL/Entities/Types/ResponseModels/UserCardCollectionResponseModelUnionType.cs`

**Steps**:
1. Create union type for collection response (multiple cards)

**Modifications** (describe changes, do not show code):
- Create class UserCardsCollectionResponseModelUnionType (note the 's')
- Inherit from UnionType<ResponseModel>
- Configure to add SuccessDataResponseModel<List<UserCardCollectionOutEntity>>
- Configure to add FailureResponseModel
- This handles returning multiple user cards

**Pattern to Follow** (reference only, no code):
- Union type pattern for GraphQL responses
- Success and failure variants
- Type resolver based on actual type

### TSK-TEST 19: Unit Tests for Response Type

**Test File Location**: `src/App.MtgDiscovery.GraphQL.Tests/Entities/Types/ResponseModels/UserCardsCollectionResponseModelUnionTypeTests.cs`

**Test Scenarios to Cover**:
- Type configuration is valid
- Success type is registered
- Failure type is registered

### TSK-IMPL 20: Register GraphQL Query in Schema

**Location**: `src/App.MtgDiscovery.GraphQL/Schemas/ApiQueryExtensions.cs`

**Reference**: Check existing file

**Steps**:
1. Add UserCardsQueryMethods to query extensions

**Modifications** (describe changes, do not show code):
- Add .AddTypeExtension<UserCardsQueryMethods>() to the query builder chain
- Maintains registration of all query extensions

**Pattern to Follow** (reference only, no code):
- Chain AddTypeExtension calls
- Register all query method classes

### TSK-TEST 20: Unit Tests for Schema Registration

**Note**: Schema registration tested through integration tests

### TSK-IMPL 21: Create Validator for UserCardsSetArgEntity

**Location**: `src/Lib.MtgDiscovery.Entry/Queries/Validators/UserCardsSetArgEntityValidator.cs`

**Reference**: Copy from `src/Lib.MtgDiscovery.Entry/Queries/Validators/SetIdsArgEntityValidator.cs`

**Steps**:
1. Create validator for set arguments
2. Validate SetId is not null/empty

**Modifications** (describe changes, do not show code):
- Create validator class implementing appropriate interface
- Validate that SetId is not null or empty
- Return validation result with appropriate failure response
- Follow existing validator patterns

**Pattern to Follow** (reference only, no code):
- Implement validator interface
- Check for null/empty values
- Return typed validation results
- Include meaningful error messages

### TSK-TEST 21: Unit Tests for UserCardsSetArgEntityValidator

**Test File Location**: `src/Lib.MtgDiscovery.Entry.Tests/Queries/Validators/UserCardsSetArgEntityValidatorTests.cs`

**Test Scenarios to Cover**:
- Valid SetId passes validation
- Null SetId fails validation
- Empty SetId fails validation
- Whitespace SetId fails validation
- Validation messages are appropriate

### TSK-IMPL 22: Create Query Parameter Binding Helper

**Location**: `src/Lib.Adapter.Scryfall.Cosmos/Apis/Operators/Inquisitions/QueryParameterExtensions.cs`

**Reference**: Look for similar parameter binding patterns

**Steps**:
1. Create extension method for adding parameters to InquiryDefinition
2. Support binding named parameters

**Modifications** (describe changes, do not show code):
- Create static class QueryParameterExtensions
- Add extension method WithParameter for InquiryDefinition
- Method should support adding parameter name and value
- Return modified inquiry definition
- This helps bind @setId parameter in queries

**Pattern to Follow** (reference only, no code):
- Extension method pattern
- Fluent interface (return modified object)
- Support Azure Cosmos parameter binding

### TSK-TEST 22: Unit Tests for QueryParameterExtensions

**Test File Location**: `src/Lib.Adapter.Scryfall.Cosmos.Tests/Apis/Operators/Inquisitions/QueryParameterExtensionsTests.cs`

**Test Scenarios to Cover**:
- WithParameter adds parameter to query
- Multiple parameters can be added
- Null parameter name throws
- Parameter values are properly bound

### TSK-IMPL 23: Create Pull Request

**Description**: Create pull request for all completed work

**Steps**:
1. Ensure all implementation tasks complete
2. Ensure all test tasks complete and passing
3. Run build and all tests
4. Create PR with comprehensive description
5. Link to User Story

**PR Description Template**:
```
## Summary
- Implemented UserCards by Set query flow from Adapter to GraphQL layer
- Added query to retrieve all user cards for a specific set
- Follows existing architectural patterns and layer separation

## Changes
- Adapter Layer: Added query adapter with Cosmos inquisition for set filtering
- Aggregator Layer: Extended to support query operation
- Domain Layer: Added query method (simple delegation for now)
- Entry Layer: Created query service with validation
- GraphQL Layer: Added authorized query method for user cards by set

## Testing
- Unit tests for all new components
- Follows existing test patterns
- All tests passing

## Query Usage
```graphql
query UserCardsBySet($setId: String!) {
  userCardsBySet(setArgs: { setId: $setId }) {
    ... on SuccessDataResponseModel {
      data {
        userId
        cardId
        setId
        collectedList {
          finish
          special
          count
        }
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
```

### TSK-IMPL 24: User Approval

**Description**: Obtain user approval for completed implementation

**Steps**:
1. Demonstrate functionality with GraphQL query
2. Show test coverage report
3. Walk through architectural layers
4. Address any feedback
5. Obtain final approval

## Implementation Checklist

### Adapter Layer
- [ ] SelectUserCardsBySetQueryDefinition created
- [ ] SelectUserCardsBySetInquisition created
- [ ] UserCardsInquisitor created
- [ ] IUserCardsQueryAdapter interface created
- [ ] UserCardsQueryAdapter implemented
- [ ] IUserCardsAdapterService updated
- [ ] UserCardsAdapterService updated

### Aggregator Layer
- [ ] IUserCardsAggregatorService updated
- [ ] UserCardsAggregatorService updated

### Domain Layer
- [ ] IUserCardsDomainService updated
- [ ] UserCardsDomainService updated

### Entry Layer
- [ ] IUserCardsQueryEntryService created
- [ ] UserCardsQueryEntryService implemented
- [ ] IEntryService updated
- [ ] EntryService updated
- [ ] Validator created

### GraphQL Layer
- [ ] UserCardsSetArgEntity created
- [ ] IUserCardsSetArgEntity interface created
- [ ] UserCardsQueryMethods created
- [ ] Response union type created
- [ ] Schema registration updated

### Testing
- [ ] All unit tests created
- [ ] All tests passing
- [ ] Coverage meets standards

### Documentation & Approval
- [ ] Pull request created
- [ ] User approval obtained

## Notes

1. **Cosmos Query Pattern**: The implementation uses a parameterized query with @setId to filter UserCardItems by their set_id field. The partition key (userId) ensures we only query within the user's partition for efficiency.

2. **Layer Separation**: Each layer maintains its responsibility:
   - Adapter: Cosmos-specific implementation
   - Aggregator: Coordination (simple delegation here)
   - Domain: Business rules (none currently)
   - Entry: Validation and auth extraction
   - GraphQL: HTTP/GraphQL concerns

3. **Security**: The query requires authentication ([Authorize] attribute) and uses the JWT claims to extract the user ID, ensuring users can only query their own cards.

4. **Testing Strategy**: Each component has dedicated unit tests following existing patterns. Use TypeWrapper for internal classes, fakes for dependencies, and self-contained test methods.

5. **Future Enhancements**:
   - Could add pagination support
   - Could add sorting options
   - Could enrich with card details from card service
   - Could add caching at aggregator layer