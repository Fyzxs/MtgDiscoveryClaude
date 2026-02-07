using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Aggregator.Collections.Mappers;

internal interface IAuthorizedUserExtToOufMapper : ICreateMapper<AuthorizedUserExtEntity, IAuthorizedUserOufEntity>;
