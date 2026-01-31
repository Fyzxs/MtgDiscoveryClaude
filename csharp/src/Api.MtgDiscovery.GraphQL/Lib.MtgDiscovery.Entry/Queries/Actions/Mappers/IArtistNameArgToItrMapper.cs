using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IArtistNameArgToItrMapper : ICreateMapper<IArtistNameArgEntity, IArtistNameItrEntity>;
