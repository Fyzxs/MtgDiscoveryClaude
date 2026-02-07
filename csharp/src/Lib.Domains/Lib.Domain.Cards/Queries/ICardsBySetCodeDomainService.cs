using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Marker interface for retrieving cards by set code.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardsBySetCodeDomainService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> Execute(
        ISetCodeItrEntity input,
        CancellationToken cancellationToken);
}
