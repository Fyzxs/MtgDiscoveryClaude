using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.Cards;

internal interface ICardsByIdsEntryService : IOperationResponseService<ICardIdsArgEntity, List<CardItemOutEntity>>;
