using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards;

internal interface IAddSetGroupToUserSetCardEntryService : IOperationResponseService<IAddSetGroupToUserSetCardArgsEntity, UserSetCardOutEntity>
{
}