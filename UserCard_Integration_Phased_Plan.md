# UserCard Integration - Phased Implementation Plan

## Overview
Add optional UserId to card retrieval methods, enabling the backend to enrich card data with UserCard collection information in a single request.

## Design Principles
- **Non-breaking changes** - All existing queries continue to work without UserId
- **Incremental implementation** - One phase at a time with validation between phases
- **Pattern establishment** - Implement one method fully, then replicate pattern
- **Direct entity reuse** - Use existing `CollectedItemOutEntity` without new structures
- **User validation points** - Pause after each phase for testing

## Phase 1: Update ArgEntities (Non-Breaking)

Add optional UserId field and helper property to all card-related ArgEntities:

### 1.1 Update ICardIdsArgEntity
```csharp
public interface ICardIdsArgEntity
{
    ICollection<string> Ids { get; }
    string UserId { get; }  // New - optional from GraphQL
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;  // New helper
}
```

### 1.2 Update ISetCodeArgEntity
```csharp
public interface ISetCodeArgEntity
{
    string SetCode { get; }
    string UserId { get; }  // New
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;  // New
}
```

### 1.3 Update ICardNameArgEntity
```csharp
public interface ICardNameArgEntity
{
    string CardName { get; }
    string UserId { get; }  // New
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;  // New
}
```

### 1.4 Update IArtistIdArgEntity
```csharp
public interface IArtistIdArgEntity
{
    string ArtistId { get; }
    string UserId { get; }  // New
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;  // New
}
```

### 1.5 Update IArtistNameArgEntity
```csharp
public interface IArtistNameArgEntity
{
    string ArtistName { get; }
    string UserId { get; }  // New
    bool HasUserId => string.IsNullOrEmpty(UserId) is false;  // New
}
```

**PAUSE POINT**: Verify all existing queries still work without UserId

## Phase 2: Update OutEntity

### 2.1 Update CardItemOutEntity (Only OutEntity that needs changes)
```csharp
public sealed class CardItemOutEntity
{
    // ... existing properties ...
    public ICollection<CollectedItemOutEntity> UserCollection { get; init; }  // New
}
```

Note: CardNameSearchResultOutEntity does NOT get updated as it only returns card names for search/autocomplete.

### 2.2 Update Mapper to Default Empty Collection

**CollectionCardItemOufToOutMapper**:
```csharp
// In Map method, add to each CardItemOutEntity:
UserCollection = new List<CollectedItemOutEntity>()  // Empty collection default
```

**PAUSE POINT**: Verify cards return with empty UserCollection arrays

## Phase 3: Single Update - CardEntryService.CardsByIdsAsync

### 3.1 Add Dependencies to CardEntryService
```csharp
private readonly IUserCardsQueryEntryService _userCardsQueryEntryService;

// Update constructor to include:
public CardEntryService(ILogger logger) : this(
    // ... existing dependencies ...
    new UserCardsQueryEntryService(logger))  // Add this
{ }

// Update private constructor to accept and assign
```

### 3.2 Update CardsByIdsAsync Method
```csharp
public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args)
{
    // Existing validation
    IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult =
        await _cardIdsArgEntityValidator.Validate(args).ConfigureAwait(false);
    if (validatorResult.IsNotValid())
        return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

    // Existing mapping and domain call
    ICardIdsItrEntity itrEntity = await _cardsArgsToItrMapper.Map(args).ConfigureAwait(false);
    IOperationResponse<ICardItemCollectionOufEntity> opResponse =
        await _cardDomainService.CardsByIdsAsync(itrEntity).ConfigureAwait(false);
    if (opResponse.IsFailure)
        return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

    // Existing OufToOut mapping
    List<CardItemOutEntity> outEntities =
        await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

    // NEW: UserCard enrichment if UserId present
    if (args.HasUserId)
    {
        // Extract card IDs
        List<string> cardIds = outEntities.Select(c => c.Id).ToList();

        // Create args for UserCard retrieval
        UserCardsByIdsArgEntity userCardsArgs = new()
        {
            UserId = args.UserId,
            CardIds = cardIds
        };

        // Retrieve UserCard information
        IOperationResponse<List<UserCardOutEntity>> userCardResponse =
            await _userCardsQueryEntryService.UserCardsByIdsAsync(userCardsArgs).ConfigureAwait(false);

        // If successful, merge data
        if (!userCardResponse.IsFailure && userCardResponse.ResponseData != null)
        {
            // Create lookup dictionary
            Dictionary<string, ICollection<CollectedItemOutEntity>> userCardLookup =
                userCardResponse.ResponseData.ToDictionary(
                    uc => uc.CardId,
                    uc => uc.CollectedList ?? new List<CollectedItemOutEntity>()
                );

            // Update cards with their collection data
            foreach (CardItemOutEntity card in outEntities)
            {
                if (userCardLookup.TryGetValue(card.Id, out ICollection<CollectedItemOutEntity> collectedItems))
                {
                    card.UserCollection = collectedItems;
                }
                // Else: keeps empty collection from mapper
            }
        }
        // Else: on failure, cards keep empty collections
    }

    return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
}
```

### 3.3 Add UserCardsByIdsArgEntity Implementation (if not exists)
```csharp
public sealed class UserCardsByIdsArgEntity : IUserCardsByIdsArgEntity
{
    public string UserId { get; init; }
    public ICollection<string> CardIds { get; init; }
}
```

**PAUSE POINT**: Test CardsByIdsAsync with and without UserId

## Phase 4: Second Update - ArtistEntryService.CardsByArtistAsync

### 4.1 Add Dependencies to ArtistEntryService
```csharp
private readonly IUserCardsQueryEntryService _userCardsQueryEntryService;

// Update constructors similar to CardEntryService
```

### 4.2 Update CardsByArtistAsync Method
```csharp
public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId)
{
    // Existing validation and retrieval...
    IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult =
        await _artistIdArgEntityValidator.Validate(artistId).ConfigureAwait(false);
    if (validatorResult.IsNotValid())
        return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

    IArtistIdItrEntity itrEntity = await _artistIdArgsToItrMapper.Map(artistId).ConfigureAwait(false);
    IOperationResponse<ICardItemCollectionOufEntity> opResponse =
        await _artistDomainService.CardsByArtistAsync(itrEntity).ConfigureAwait(false);
    if (opResponse.IsFailure)
        return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

    List<CardItemOutEntity> outEntities =
        await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);

    // NEW: Same UserCard enrichment pattern
    if (artistId.HasUserId)
    {
        List<string> cardIds = outEntities.Select(c => c.Id).ToList();

        UserCardsByIdsArgEntity userCardsArgs = new()
        {
            UserId = artistId.UserId,
            CardIds = cardIds
        };

        IOperationResponse<List<UserCardOutEntity>> userCardResponse =
            await _userCardsQueryEntryService.UserCardsByIdsAsync(userCardsArgs).ConfigureAwait(false);

        if (!userCardResponse.IsFailure && userCardResponse.ResponseData != null)
        {
            Dictionary<string, ICollection<CollectedItemOutEntity>> userCardLookup =
                userCardResponse.ResponseData.ToDictionary(
                    uc => uc.CardId,
                    uc => uc.CollectedList ?? new List<CollectedItemOutEntity>()
                );

            foreach (CardItemOutEntity card in outEntities)
            {
                if (userCardLookup.TryGetValue(card.Id, out ICollection<CollectedItemOutEntity> collectedItems))
                {
                    card.UserCollection = collectedItems;
                }
            }
        }
    }

    return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
}
```

**PAUSE POINT**: Verify pattern works in second domain

## Phase 5: Apply Pattern to Remaining Methods

With the pattern established, apply to:

### 5.1 CardEntryService
- `CardsBySetCodeAsync`
- `CardsByNameAsync`

### 5.2 ArtistEntryService
- `CardsByArtistNameAsync`

Each follows the same pattern:
1. Check `args.HasUserId`
2. Extract card IDs from outEntities
3. Create UserCardsByIdsArgEntity
4. Call UserCardsQueryEntryService
5. If successful, create lookup dictionary
6. Update each card's UserCollection

**PAUSE POINT**: Full testing of all methods

## GraphQL Updates (Optional Phase 6)

Once core functionality verified:

### 6.1 Update Input Types
Add optional UserId field to GraphQL input types

### 6.2 Update Output Types
Add UserCollection field to GraphQL output types

### 6.3 Update Query Methods
Pass authenticated UserId when available:
```csharp
if (authenticatedUser.IsAuthenticated && string.IsNullOrEmpty(args.UserId))
{
    args = args with { UserId = authenticatedUser.UserId };
}
```

## Success Criteria

- ✅ All existing queries work without UserId
- ✅ Queries with UserId return UserCollection data
- ✅ Failed UserCard lookups gracefully degrade to empty collections
- ✅ Pattern consistent across all card retrieval methods
- ✅ No N+1 queries - single batch request for UserCard data

## Key Differences from Original Plan

1. **No new intermediate entities** - Reuse `CollectedItemOutEntity` directly
2. **No separate enrichment service** - Logic inline in EntryService methods
3. **HasUserId property** - Clean check without null comparisons everywhere
4. **Empty collection default** - Simpler than complex default objects
5. **Phased with pause points** - Verify each phase before proceeding

## Notes

- The `HasUserId` property keeps the check clean and consistent
- Empty collection `[]` is a valid response - frontend can check `.length`
- Pattern is simple enough to copy-paste with minor modifications
- Each phase is independently testable
- Graceful degradation built into each method