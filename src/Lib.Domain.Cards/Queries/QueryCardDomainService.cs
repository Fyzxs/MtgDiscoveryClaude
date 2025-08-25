using System.Threading.Tasks;
using Lib.Aggregator.Cards.Apis;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.Cards.Queries;

internal sealed class QueryCardDomainService : ICardDomainService
{
    private readonly ICardAggregatorService _cardAggregatorService;

    public QueryCardDomainService(ILogger logger) : this(new CardAggregatorService(logger))
    {

    }

    private QueryCardDomainService(ICardAggregatorService cardAggregatorService)
    {
        _cardAggregatorService = cardAggregatorService;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args)
    {
        return await _cardAggregatorService.CardsByIdsAsync(args).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode)
    {
        return await _cardAggregatorService.CardsBySetCodeAsync(setCode).ConfigureAwait(false);
    }
}
