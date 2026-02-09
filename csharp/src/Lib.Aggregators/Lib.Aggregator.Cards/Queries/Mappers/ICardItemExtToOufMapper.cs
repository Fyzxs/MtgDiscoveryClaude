using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardItemExtToOufMapper : ICreateMapper<ScryfallCardItemExtEntity, ICardItemOufEntity>;
