using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardsByName;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Cards.Queries.Mappers;

internal interface ICardByNameExtToOufMapper : ICreateMapper<ScryfallCardByNameExtEntity, ICardItemOufEntity>;
