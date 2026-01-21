using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal interface ICollectedItemMapper : ICreateMapper<ICollectedItemItrEntity, CollectedCardInfoExtArg>;
