using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Domain.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSetCards.Queries;

internal sealed class QueryUserSetCardsDomainService : IUserSetCardsQueryDomainService
{
    private readonly IUserSetCardsAggregatorService _userSetCardsAggregatorService;

    public QueryUserSetCardsDomainService(ILogger logger) : this(new UserSetCardsAggregatorService(logger))
    { }

    private QueryUserSetCardsDomainService(IUserSetCardsAggregatorService userSetCardsAggregatorService) => _userSetCardsAggregatorService = userSetCardsAggregatorService;

    public async Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => await _userSetCardsAggregatorService.GetUserSetCardByUserAndSetAsync(userSetCard).ConfigureAwait(false);
}
