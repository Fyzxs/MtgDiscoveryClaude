# User Cards Collection Implementation Plan

## Overview
This document outlines the implementation plan for completing the "Add Card to User Collection" feature across all architectural layers of the MTG Discovery Vibe platform, following the CQRS command pattern and MicroObjects principles.

## Architecture Flow
**Request Flow:** GraphQL → Entry → Domain → Aggregator → Adapter → Cosmos DB
**Response Flow:** Cosmos DB → Adapter → Aggregator → Domain → Entry → GraphQL

## Current State Assessment

### ✅ Completed Components
1. **Shared Layer (Lib.Shared.DataModels)**
   - `IUserCardCollectionItrEntity` - Main entity interface
   - `ICollectedItemItrEntity` - Collection item details

2. **Adapter Layer (Lib.Adapter.UserCards)**
   - `IUserCardsCommandAdapter` - Command adapter interface
   - `IUserCardsAdapterService` - Main adapter service interface
   - `UserCardsCommandAdapter` - Implementation (partial)
   - `UserCardsAdapterService` - Service implementation (partial)
   - `UserCardsAdapterException` - Exception handling

3. **Storage Layer (Lib.Adapter.Scryfall.Cosmos)**
   - `UserCardsScribe` - Cosmos DB operations
   - `UserCardsCosmosContainer` - Container configuration
   - `UserCardsCosmosContainerDefinition` - Container definition

### ❌ Missing Components
1. **GraphQL Layer** - No mutation for adding cards to collection
2. **Entry Layer** - No UserCards entry service
3. **Domain Layer** - No UserCards domain service
4. **Aggregator Layer** - Empty project, no implementation

## User Stories by Layer (Bottom-Up Implementation)

### User Story 1: Adapter Layer - Complete Command Implementation
**As an** Adapter layer service
**I want to** persist user card collections to Cosmos DB
**So that** user collections are permanently stored

#### Acceptance Criteria
- [ ] Complete `AddUserCardAsync` implementation
- [ ] Map ItrEntity to Cosmos storage model
- [ ] Handle upsert operations
- [ ] Proper error handling with `UserCardsAdapterException`
- [ ] Returns operation response pattern

#### Implementation Tasks
1. Complete `UserCardsCommandAdapter.AddUserCardAsync` implementation
2. Create storage entity models:
   - `UserCardCollectionItem` for Cosmos DB
   - `CollectedItem` for nested collection details
3. Implement entity mapping (ItrEntity → ExtEntity)
4. Handle Cosmos DB operations via `UserCardsScribe`
5. Implement proper exception handling and response wrapping

---

### User Story 2: Aggregator Layer - UserCards Aggregator Service
**As an** Aggregator layer service
**I want to** coordinate user card data operations
**So that** data from multiple sources is properly aggregated

#### Acceptance Criteria
- [ ] `IUserCardsAggregatorService` interface defined
- [ ] Coordinates with UserCards adapter
- [ ] Handles data transformation between layers
- [ ] Manages upsert logic for existing collections
- [ ] Returns operation response pattern

#### Implementation Tasks
1. Create `IUserCardsAggregatorService` in `Lib.Aggregator.UserCards/Apis/`
2. Implement `UserCardsAggregatorService` class
3. Coordinate with adapter for:
   - Check if user-card combination exists
   - Update existing collection or create new
   - Handle collection item merging
4. Transform entities between domain and storage models
5. Handle concurrent update scenarios

---

### User Story 3: Domain Layer - UserCards Domain Service
**As a** Domain layer service
**I want to** apply business logic to user card collection operations
**So that** domain rules and constraints are enforced

#### Acceptance Criteria
- [ ] Project `Lib.Domain.UserCards` created
- [ ] `IUserCardsDomainService` interface defined
- [ ] Business rules applied (e.g., max collection size, valid finishes)
- [ ] Coordinates with aggregator service
- [ ] Returns operation response pattern

#### Implementation Tasks
1. Create new project `Lib.Domain.UserCards`
2. Define `IUserCardsDomainService` interface
3. Implement `UserCardsDomainService` class
4. Apply business rules:
   - Validate card exists (coordinate with Cards domain)
   - Validate user exists
   - Check collection limits
   - Validate finish types ("nonfoil", "foil", "etched")
   - Validate special types ("none", "altered", "signed", "proof")
5. Add project reference in solution
6. Configure dependency injection

---

### User Story 4: Entry Layer - UserCards Entry Service
**As an** Entry layer service
**I want to** validate and coordinate user card collection commands
**So that** business rules are enforced before processing

#### Acceptance Criteria
- [ ] `IUserCardsEntryService` interface defined
- [ ] Input validation for collection arguments
- [ ] Argument to ItrEntity mapping
- [ ] Delegates to domain service
- [ ] Returns operation response pattern
- [ ] Integrated into main `IEntryService`

#### Implementation Tasks
1. Create `IUserCardsEntryService` interface in `Lib.MtgDiscovery.Entry/Apis/`
2. Implement `UserCardsEntryService` class
3. Create validators:
   - `AddCardToCollectionArgEntityValidatorContainer`
   - Individual validators for null checks, count validation, finish validation
4. Create mapper: `AddCardToCollectionArgsToItrMapper`
5. Update `IEntryService` to inherit from `IUserCardsEntryService`
6. Update `EntryService` implementation to include UserCards operations

---

### User Story 5: GraphQL Layer - Add Card to Collection Mutation
**As a** GraphQL API consumer
**I want to** call a mutation to add a card to my collection
**So that** I can track my card ownership

#### Acceptance Criteria
- [ ] Authenticated mutation `addCardToCollection` exists
- [ ] Accepts card ID, set ID, and collection details (finish, special, count)
- [ ] Returns success with collection data or failure with error details
- [ ] JWT authentication required with user claims extraction
- [ ] Union type response (success/failure) pattern

#### Implementation Tasks
1. Create `UserCardsMutationMethods.cs` in `App.MtgDiscovery.GraphQL/Mutations/`
2. Define `AddCardToCollectionArgEntity` for input arguments
3. Create `UserCardCollectionOutEntity` for response data
4. Define GraphQL ObjectType for user card collection
5. Implement union type response model
6. Add authentication and claims principal injection
7. Wire up to IEntryService

---

## Technical Constraints

### MicroObjects Principles
- No primitives (except in DTOs)
- Immutable objects with private readonly fields
- Interface for every class (1:1 mapping)
- No nulls - use Null Object pattern
- Constructor injection only
- No public statics, no enums

### Coding Standards
- File-scoped namespaces
- `ConfigureAwait(false)` on all async calls
- Operation response pattern for all service methods
- No boolean negation - use `is false`
- DTOs use `init` setters

### Testing Requirements
- Unit tests for each component
- Self-contained tests (no shared state)
- Fake implementations over mocks
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- TypeWrapper pattern for classes with private constructors

## Implementation Order (Bottom-Up Approach)

1. **Adapter Layer Completion** - Finish command adapter implementation (foundation)
2. **Aggregator Layer** (Lib.Aggregator.UserCards) - Implement data coordination
3. **Domain Layer** (Lib.Domain.UserCards) - Define business logic contract
4. **Entry Layer** (Update Lib.MtgDiscovery.Entry) - Add validation and coordination
5. **GraphQL Layer** (Update App.MtgDiscovery.GraphQL) - Expose mutation endpoint (final API)
6. **Integration Testing** - End-to-end testing of the complete flow

**Rationale for Bottom-Up**:
- Each layer can be fully tested before the layer above it is implemented
- Dependencies flow upward, so lower layers don't need to mock upper layers
- Adapter layer provides the foundation for actual data persistence
- GraphQL layer is implemented last when all supporting infrastructure is ready

## Success Criteria

- [ ] User can add a card to their collection via GraphQL mutation
- [ ] Authentication enforces user ownership
- [ ] Collection updates handle existing entries (merge counts)
- [ ] All finish types supported (nonfoil, foil, etched)
- [ ] All special types supported (none, altered, signed, proof)
- [ ] Proper error handling at each layer
- [ ] Unit tests pass for all components
- [ ] Integration test demonstrates full flow

## Questions for Clarification

1. **Collection Limits**: Should there be a maximum number of cards in a collection or maximum count per card?
2. **Duplicate Handling**: When adding a card that already exists, should we:
   - Merge the counts for same finish/special combination?
   - Replace the existing entry?
   - Return an error?
3. **Validation Scope**: Should we validate that the CardId and SetId actually exist in the system, or trust the client?
4. **Transaction Support**: Should adding multiple cards be atomic (all succeed or all fail)?
5. **Audit Trail**: Should we track when cards were added to the collection (timestamps)?

## Next Steps

After review and approval of this plan:
1. Create detailed task breakdown for each user story
2. Implement components in the specified order
3. Write unit tests alongside implementation
4. Perform integration testing
5. Update documentation and CLAUDE.md files