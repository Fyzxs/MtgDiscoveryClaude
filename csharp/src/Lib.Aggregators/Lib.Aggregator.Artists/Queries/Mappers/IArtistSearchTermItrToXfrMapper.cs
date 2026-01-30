using Lib.Adapter.Artists.Apis.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistSearchTermItrToXfrMapper : ICreateMapper<IArtistSearchTermItrEntity, IArtistSearchTermXfrEntity>;
