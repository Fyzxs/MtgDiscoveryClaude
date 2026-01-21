using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal interface ICardsByArtistEntryService : IOperationResponseService<IArtistIdArgEntity, List<CardItemOutEntity>>
{
}
