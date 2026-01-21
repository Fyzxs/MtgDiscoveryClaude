using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal interface ICardsByIdsEntryService : IOperationResponseService<ICardIdsArgEntity, List<CardItemOutEntity>>;
