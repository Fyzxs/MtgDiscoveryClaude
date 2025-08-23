using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

internal sealed class CardDomainOperations : ICardDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public CardDomainOperations(ILogger logger) : this(new CardAggregatorService(logger))
    {

    }

    private CardDomainOperations(ICardAggregatorService cardAggregatorService)
    {
        _cardAggregatorService = cardAggregatorService;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return await _cardAggregatorService.CardsByIdsAsync(args).ConfigureAwait(false);
    }
}
