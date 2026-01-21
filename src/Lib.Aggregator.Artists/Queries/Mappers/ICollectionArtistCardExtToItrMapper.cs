using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface ICollectionArtistCardExtToItrMapper : ICreateMapper<IEnumerable<ScryfallArtistCardExtEntity>, IEnumerable<ICardItemItrEntity>>
{
}
