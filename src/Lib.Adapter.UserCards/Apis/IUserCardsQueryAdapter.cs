using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Apis;

/// <summary>
/// Specialized adapter interface for user card collection query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main IUserCardsAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   IUserCardsAdapterService : IUserCardsCommandAdapter, IUserCardsQueryAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to ItrEntity
/// </summary>
public interface IUserCardsQueryAdapter
{
    /// <summary>
    /// Retrieves all user cards for a specific user within a given set.
    /// </summary>
    /// <param name="userCardsSet">The user cards set entity containing userId and setId</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(IUserCardsSetXfrEntity userCardsSet);

    /// <summary>
    /// Retrieves a specific user card using point read operation.
    /// </summary>
    /// <param name="userCard">The user card entity containing userId and cardId</param>
    /// <returns>Collection containing zero or one user card wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardAsync(IUserCardXfrEntity userCard);

    /// <summary>
    /// Retrieves multiple user cards using parallel point read operations.
    /// </summary>
    /// <param name="userCards">The user cards entity containing userId and collection of cardIds</param>
    /// <returns>Collection of found user cards wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByIdsAsync(IUserCardsByIdsXfrEntity userCards);

    /// <summary>
    /// Retrieves all user cards for a specific artist.
    /// </summary>
    /// <param name="userCardsArtist">The user cards artist entity containing userId and artistId</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByArtistAsync(IUserCardsArtistXfrEntity userCardsArtist);

    /// <summary>
    /// Retrieves all user cards with a specific card name.
    /// </summary>
    /// <param name="userCardsName">The user cards name entity containing userId and cardName</param>
    /// <returns>Collection of user card collection information wrapped in an operation response</returns>
    Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByNameAsync(IUserCardsNameXfrEntity userCardsName);
}
