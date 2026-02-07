using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Marker interface for retrieving cards by ID collection.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByIdsDomainService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ICardIdsItrEntity input,
        CancellationToken cancellationToken);
}
