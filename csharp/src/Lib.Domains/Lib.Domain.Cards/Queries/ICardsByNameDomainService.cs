using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Marker interface for retrieving cards by name.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsByNameDomainService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ICardNameItrEntity input,
        CancellationToken cancellationToken);
}
