using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Shared.Abstractions.Integrators;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Adapter.UserSetCards.Commands.Integrators;

internal interface IAddSetGroupIntegrator : IIntegrator<UserSetCardExtEntity, IAddSetGroupToUserSetCardXfrEntity>;
