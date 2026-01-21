using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardItemExtToItrMapper : ICreateMapper<ScryfallCardItemExtEntity, ICardItemItrEntity>;
