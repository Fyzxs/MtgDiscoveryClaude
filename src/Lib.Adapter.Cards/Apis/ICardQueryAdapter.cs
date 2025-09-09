using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities;
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
/// Entity Mapping: Preserves ItrEntity parameters following MicroObjects principles.
/// Primitive extraction happens in the concrete implementation when interfacing with external systems.
/// </summary>
public interface ICardQueryAdapter
{
    Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsByIdsAsync(ICardIdsItrEntity cardIds);
    Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsBySetCodeAsync(ISetCodeItrEntity setCode);
    Task<IOperationResponse<IEnumerable<ICardItemItrEntity>>> GetCardsByNameAsync(ICardNameItrEntity cardName);
    Task<IOperationResponse<IEnumerable<ICardNameSearchResultItrEntity>>> SearchCardNamesAsync(ICardSearchTermItrEntity searchTerm);
}