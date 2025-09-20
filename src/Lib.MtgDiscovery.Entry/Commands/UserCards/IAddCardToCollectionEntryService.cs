using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Outs.UserCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

internal interface IAddCardToCollectionEntryService : IOperationResponseService<AddCardToCollectionArgsEntity, UserCardOutEntity>
{
}
