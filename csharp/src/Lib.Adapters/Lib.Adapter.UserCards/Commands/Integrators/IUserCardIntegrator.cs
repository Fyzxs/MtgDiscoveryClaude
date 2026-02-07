using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Integrators;

namespace Lib.Adapter.UserCards.Commands.Integrators;

internal interface IUserCardIntegrator : IIntegrator<UserCardExtEntity, IAddUserCardXfrEntity>;
