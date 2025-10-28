using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Adapter.UserSetCards.Commands;

/// <summary>
/// Adapter for adding a set group to a user's set collection tracking.
/// Implements atomic read-modify-write pattern.
/// </summary>
internal interface IAddSetGroupToUserSetCardAdapter
    : IOperationResponseService<IAddSetGroupToUserSetCardXfrEntity, UserSetCardExtEntity>;
