using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal interface ICardsByArtistEntryService : IOperationResponseService<IArtistIdArgEntity, List<CardItemOutEntity>>
{
}
