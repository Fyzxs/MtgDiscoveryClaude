using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IArtistNameArgToUserCardsArtistContextCollectionMapper : ICreateMapper<IArtistNameArgEntity, List<CardItemOutEntity>, IEnumerable<IUserCardsArtistItrEntity>>
{
}
