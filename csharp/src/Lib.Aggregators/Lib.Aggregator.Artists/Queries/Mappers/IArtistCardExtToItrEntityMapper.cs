using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Cards;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistCardExtToItrEntityMapper : ICreateMapper<ScryfallArtistCardExtEntity, ICardItemItrEntity>;
