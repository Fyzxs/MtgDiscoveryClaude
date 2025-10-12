using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

internal interface IAddCardToCollectionEntryService : IOperationResponseService<IAddCardToCollectionArgsEntity, List<CardItemOutEntity>>
{
}
