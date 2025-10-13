using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal interface IAddSetGroupToUserSetCardArgsMapper : ICreateMapper<IAuthUserArgEntity, IAddSetGroupToUserSetCardArgEntity, IAddSetGroupToUserSetCardArgsEntity>
{
}
