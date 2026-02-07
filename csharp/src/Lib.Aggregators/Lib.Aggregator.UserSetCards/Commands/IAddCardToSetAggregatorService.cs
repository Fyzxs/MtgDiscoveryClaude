using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserSetCards.Commands;

/// <summary>
/// Single-method aggregator service for adding a card to a user's set collection.
/// Used by migration tools to directly update UserSetCards without affecting UserCards.
/// </summary>
internal interface IAddCardToSetAggregatorService
{
    Task<IOperationResponse<IUserSetCardOufEntity>> Execute(
        IAddCardToSetItrEntity input,
        CancellationToken cancellationToken);
}
