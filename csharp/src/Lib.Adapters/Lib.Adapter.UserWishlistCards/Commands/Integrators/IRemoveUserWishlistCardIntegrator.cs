using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserWishlistCards;
using Lib.Adapter.UserWishlistCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Integrators;

namespace Lib.Adapter.UserWishlistCards.Commands.Integrators;

internal interface IRemoveUserWishlistCardIntegrator
    : IIntegrator<UserWishlistCardExtEntity, IRemoveUserWishlistCardXfrEntity>;
