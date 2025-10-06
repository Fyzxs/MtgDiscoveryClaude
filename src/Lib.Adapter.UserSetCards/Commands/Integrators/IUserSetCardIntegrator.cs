using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserSetCards.Apis.Entities;
using Lib.Shared.Abstractions.Integrators;

namespace Lib.Adapter.UserSetCards.Commands.Integrators;

internal interface IUserSetCardIntegrator : IIntegrator<UserSetCardExtEntity, IAddCardToSetXfrEntity>;
