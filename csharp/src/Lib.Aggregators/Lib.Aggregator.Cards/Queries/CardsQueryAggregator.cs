using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
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

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsItrEntity args) => await _cardsByIdsOperations.Execute(args);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode) => await _cardsBySetCodeOperations.Execute(setCode);

    public async Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameItrEntity cardName) => await _cardsByNameOperations.Execute(cardName);

    public async Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm) => await _cardNameSearchOperations.Execute(searchTerm);
}
