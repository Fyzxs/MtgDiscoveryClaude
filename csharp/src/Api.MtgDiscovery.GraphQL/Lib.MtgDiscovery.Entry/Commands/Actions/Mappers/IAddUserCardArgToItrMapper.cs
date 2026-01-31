using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal interface IAddUserCardArgToItrMapper : ICreateMapper<IAddCardToCollectionArgsEntity, IUserCardItrEntity>;
