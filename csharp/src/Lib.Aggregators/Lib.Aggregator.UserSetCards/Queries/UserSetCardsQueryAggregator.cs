using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserSetCards.Queries;

internal sealed class UserSetCardsQueryAggregator : IUserSetCardsQueryAggregator
{
    private readonly IUserSetCardAggregatorService _getUserSetCardOperations;
    private readonly IAllUserSetCardsAggregatorService _getAllUserSetCardsOperations;

    public UserSetCardsQueryAggregator(ILogger logger) : this(
        new UserSetCardAggregatorService(logger),
        new AllUserSetCardsAggregatorService(logger))
    {
    }

    private UserSetCardsQueryAggregator(
        IUserSetCardAggregatorService getUserSetCardOperations,
        IAllUserSetCardsAggregatorService getAllUserSetCardsOperations)
    {
        _getUserSetCardOperations = getUserSetCardOperations;
        _getAllUserSetCardsOperations = getAllUserSetCardsOperations;
    }

    public async Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(
        IUserSetCardItrEntity userSetCard,
        CancellationToken cancellationToken) => await _getUserSetCardOperations.Execute(userSetCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(
        IAllUserSetCardsItrEntity userSetCards,
        CancellationToken cancellationToken) => await _getAllUserSetCardsOperations.Execute(userSetCards, cancellationToken).ConfigureAwait(false);
}
