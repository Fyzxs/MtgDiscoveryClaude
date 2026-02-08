using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Domain.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSetCards.Queries;

internal sealed class UserSetCardsQueryDomainService : IUserSetCardsQueryDomainService
{
    private readonly IUserSetCardsAggregatorService _userSetCardsAggregatorService;

    public UserSetCardsQueryDomainService(ILogger logger) : this(new UserSetCardsAggregatorService(logger))
    {
    }

    private UserSetCardsQueryDomainService(IUserSetCardsAggregatorService userSetCardsAggregatorService) => _userSetCardsAggregatorService = userSetCardsAggregatorService;

    public async Task<IOperationResponse<IUserSetCardOufEntity>> UserSetCardByUserAndSetAsync(
        IUserSetCardItrEntity userSetCard,
        CancellationToken cancellationToken) => await _userSetCardsAggregatorService.UserSetCardByUserAndSetAsync(userSetCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserSetCardOufEntity>>> AllUserSetCardsAsync(
        IAllUserSetCardsItrEntity userSetCards,
        CancellationToken cancellationToken) => await _userSetCardsAggregatorService.AllUserSetCardsAsync(userSetCards, cancellationToken).ConfigureAwait(false);
}
