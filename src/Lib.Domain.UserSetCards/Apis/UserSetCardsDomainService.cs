using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSetCards.Apis;

public sealed class UserSetCardsDomainService : IUserSetCardsDomainService
{
    private readonly IUserSetCardsAggregatorService _userSetCardsAggregatorService;

    public UserSetCardsDomainService(ILogger logger) : this(new UserSetCardsAggregatorService(logger))
    { }

    private UserSetCardsDomainService(IUserSetCardsAggregatorService userSetCardsAggregatorService) => _userSetCardsAggregatorService = userSetCardsAggregatorService;

    public async Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => await _userSetCardsAggregatorService.GetUserSetCardByUserAndSetAsync(userSetCard).ConfigureAwait(false);
}
