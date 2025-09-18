using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistCardExtToItrEntityMapper : ICreateMapper<ScryfallArtistCardExtEntity, ICardItemItrEntity>;
