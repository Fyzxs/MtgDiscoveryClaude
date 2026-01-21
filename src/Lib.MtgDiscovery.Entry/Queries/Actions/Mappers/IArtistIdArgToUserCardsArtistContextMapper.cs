using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IArtistIdArgToUserCardsArtistContextMapper : ICreateMapper<IArtistIdArgEntity, IUserCardsArtistItrEntity>
{
}
