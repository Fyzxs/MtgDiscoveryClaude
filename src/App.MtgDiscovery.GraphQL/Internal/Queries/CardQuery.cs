using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Apis.Types;
using App.MtgDiscovery.GraphQL.Internal.Mappers;
using HotChocolate.Types;
using Lib.Adapter.Scryfall.Cosmos.Apis.Entities;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Extensions.Logging;
#pragma warning disable CA1515

namespace App.MtgDiscovery.GraphQL.Internal.Queries;

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

    public async Task<ScryfallCardEntity> CardById(string id)
    {
        ReadPointItem pointItem = new()
        {
            Id = new ProvidedCosmosItemId(id),
            Partition = new ProvidedPartitionKeyValue(id)
        };

        OpResponse<ScryfallCardItem> response = await _cardGopher
            .ReadAsync<ScryfallCardItem>(pointItem)
            .ConfigureAwait(false);

        ScryfallCardEntity scryfallCardEntity = _scryfallCardMapper.Map(response.Value);

        return scryfallCardEntity;
    }

    public async Task<IEnumerable<ScryfallCardEntity>> CardsById(IEnumerable<string> ids)
    {

        List<Task<ScryfallCardEntity>> tasks = [];

        tasks.AddRange(ids.Select(CardById));

        if (tasks.Count == 0) return [];

        ScryfallCardEntity[] results = await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Where(card => card != null);
    }
}
