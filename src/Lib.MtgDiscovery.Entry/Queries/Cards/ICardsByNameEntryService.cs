using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal interface ICardsByNameEntryService : IOperationResponseService<ICardNameArgEntity, List<CardItemOutEntity>>;
