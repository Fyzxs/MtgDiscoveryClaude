using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Aggregator.Cards.Queries.CardNameSearch;
using Lib.Aggregator.Cards.Queries.CardsByIds;
using Lib.Aggregator.Cards.Queries.CardsByName;
using Lib.Aggregator.Cards.Queries.CardsBySetCode;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.Cards.Queries;

internal sealed class CardsQueryAggregator : ICardAggregatorService
{
    private readonly ICardsByIdsAggregatorService _cardsByIdsOperations;
    private readonly ICardsBySetCodeAggregatorService _cardsBySetCodeOperations;
    private readonly ICardsByNameAggregatorService _cardsByNameOperations;
    private readonly ICardNameSearchAggregatorService _cardNameSearchOperations;

    public CardsQueryAggregator(ILogger logger) : this(
        new CardsByIdsAggregatorService(logger),
        new CardsBySetCodeAggregatorService(logger),
        new CardsByNameAggregatorService(logger),
        new CardNameSearchAggregatorService(logger))
    { }

    private CardsQueryAggregator(
        ICardsByIdsAggregatorService cardsByIdsOperations,
        ICardsBySetCodeAggregatorService cardsBySetCodeOperations,
        ICardsByNameAggregatorService cardsByNameOperations,
        ICardNameSearchAggregatorService cardNameSearchOperations)
    {
        _cardsByIdsOperations = cardsByIdsOperations;
        _cardsBySetCodeOperations = cardsBySetCodeOperations;
        _cardsByNameOperations = cardsByNameOperations;
        _cardNameSearchOperations = cardNameSearchOperations;
    }

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args) => _cardsByIdsOperations.CardsByIdsAsync(args);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode) => _cardsBySetCodeOperations.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName) => _cardsByNameOperations.CardsByNameAsync(cardName);

    public Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm) => _cardNameSearchOperations.CardNameSearchAsync(searchTerm);
}
