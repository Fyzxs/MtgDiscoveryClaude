using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserSetCards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Queries.Mappers;

internal interface ICollectionUserSetCardExtToOufMapper
    : ICreateMapper<IEnumerable<UserSetCardExtEntity>, IEnumerable<IUserSetCardOufEntity>>;
