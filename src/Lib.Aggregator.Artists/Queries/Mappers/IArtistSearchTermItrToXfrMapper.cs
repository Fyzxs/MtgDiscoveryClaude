using Lib.Adapter.Artists.Apis.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal interface IArtistSearchTermItrToXfrMapper : ICreateMapper<IArtistSearchTermItrEntity, IArtistSearchTermXfrEntity>;
