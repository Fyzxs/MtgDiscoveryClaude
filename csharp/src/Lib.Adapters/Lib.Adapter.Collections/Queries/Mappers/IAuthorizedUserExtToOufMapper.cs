using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Collections;

namespace Lib.Adapter.Collections.Queries.Mappers;

internal interface IAuthorizedUserExtToOufMapper : ICreateMapper<AuthorizedUserExtEntity, IAuthorizedUserOufEntity>;
