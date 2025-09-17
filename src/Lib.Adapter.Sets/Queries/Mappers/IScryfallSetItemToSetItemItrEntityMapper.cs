using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.Sets.Queries.Mappers;

internal interface IScryfallSetItemToSetItemItrEntityMapper : ICreateMapper<ScryfallSetItem, ISetItemItrEntity>;
