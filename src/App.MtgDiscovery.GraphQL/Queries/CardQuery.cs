using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Outs;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate.Types;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;
#pragma warning disable CA1515

namespace App.MtgDiscovery.GraphQL.Queries;

public class CardQuery;

[ExtendObjectType(typeof(CardQuery))]
public class CardQueryMethods
{
    private readonly ICosmosGopher _cardGopher;
    private readonly IScryfallCardMapper _scryfallCardMapper;

    public CardQueryMethods(ILogger logger) : this(new ScryfallCardItemsGopher(logger), new ScryfallCardMapper())
    {
    }

    private CardQueryMethods(ICosmosGopher cardGopher, IScryfallCardMapper scryfallCardMapper)
    {
        _cardGopher = cardGopher;
        _scryfallCardMapper = scryfallCardMapper;
    }

    public string Test() => "Card query endpoint is working!";

    public async Task<ScryfallCardOutEntity> CardById(string id)
    {
        ReadPointItem pointItem = new()
        {
            Id = new ProvidedCosmosItemId(id),
            Partition = new ProvidedPartitionKeyValue(id)
        };

        OpResponse<ScryfallCardItem> response = await _cardGopher
            .ReadAsync<ScryfallCardItem>(pointItem)
            .ConfigureAwait(false);

        ScryfallCardOutEntity scryfallCardOutEntity = _scryfallCardMapper.Map(response.Value);

        return scryfallCardOutEntity;
    }

    public async Task<IEnumerable<ScryfallCardOutEntity>> CardsById(CardIdsArgEntity ids)
    {

        List<Task<ScryfallCardOutEntity>> tasks = [];

        if (ids == null) return [];
        if (ids.CardIds.Count == 0) return [];

        tasks.AddRange(ids.CardIds.Select(CardById));

        if (tasks.Count == 0) return [];

        ScryfallCardOutEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Where(card => card != null);
    }
}
