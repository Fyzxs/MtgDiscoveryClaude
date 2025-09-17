using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardByNameExtToItrMapper : ICreateMapper<ScryfallCardByNameExtEntity, ICardItemItrEntity>;
