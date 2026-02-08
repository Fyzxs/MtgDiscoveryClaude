using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Artists.Queries;

internal interface IArtistSearchDomain
    : IOperationResponseService<IArtistSearchTermItrEntity, IArtistSearchResultCollectionOufEntity>;
