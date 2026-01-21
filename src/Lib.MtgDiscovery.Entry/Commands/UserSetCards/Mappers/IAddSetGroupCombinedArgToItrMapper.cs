using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal interface IAddSetGroupCombinedArgToItrMapper : ICreateMapper<IAddSetGroupToUserSetCardArgsEntity, IAddSetGroupToUserSetCardItrEntity>
{
}
