using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IArtistIdArgToUserCardsArtistContextMapper : ICreateMapper<IArtistIdArgEntity, IUserCardsArtistItrEntity>
{
}
