using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Cards.Apis;

/// <summary>
/// Specialized adapter interface for card query operations.
///
/// This interface represents the query-specific adapter functionality,
/// separate from the main ICardAdapterService which coordinates all adapters.
///
/// Pattern: Main service inherits from specialized interfaces
///   ICardAdapterService : ICardQueryAdapter, ICardCacheAdapter, ICardMetricsAdapter
///
/// Design Decision: Public specialized interface
/// While concrete implementations are internal, the specialized interfaces are public
/// to allow the main service interface to inherit from them and provide a unified API.
///
/// Entity Mapping Approach:
/// - Input: Uses XfrEntity parameters following the layered architecture pattern
/// - Output: Returns ExtEntity types from storage systems
/// - Aggregator layer handles mapping from ItrEntity to XfrEntity and ExtEntity to ItrEntity
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface ICardQueryAdapter
{
    Task<IOperationResponse<IEnumerable<ScryfallCardItemExtEntity>>> GetCardsByIdsAsync(ICardIdsXfrEntity cardIds);
    Task<IOperationResponse<IEnumerable<ScryfallSetCardItemExtEntity>>> GetCardsBySetCodeAsync(ISetCodeXfrEntity setCode);
    Task<IOperationResponse<IEnumerable<ScryfallCardByNameExtEntity>>> GetCardsByNameAsync(ICardNameXfrEntity cardName);
    Task<IOperationResponse<IEnumerable<string>>> SearchCardNamesAsync(ICardSearchTermXfrEntity searchTerm);
}
