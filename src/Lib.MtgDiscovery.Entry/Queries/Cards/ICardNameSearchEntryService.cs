using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal interface ICardNameSearchEntryService : IOperationResponseService<ICardSearchTermArgEntity, List<CardNameSearchResultOutEntity>>
{
}
