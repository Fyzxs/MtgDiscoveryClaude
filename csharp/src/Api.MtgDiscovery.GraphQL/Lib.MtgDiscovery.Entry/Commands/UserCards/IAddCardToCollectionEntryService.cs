using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards;

internal interface IAddCardToCollectionEntryService
    : IOperationResponseService<IAddCardToCollectionArgsEntity, List<CardItemOutEntity>>;
