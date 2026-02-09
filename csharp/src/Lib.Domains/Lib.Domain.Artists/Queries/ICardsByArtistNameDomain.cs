using Lib.Shared.DataModels.Entities.Itrs.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Artists.Queries;

internal interface ICardsByArtistNameDomain
    : IOperationResponseService<IArtistNameItrEntity, ICardItemCollectionOufEntity>;
