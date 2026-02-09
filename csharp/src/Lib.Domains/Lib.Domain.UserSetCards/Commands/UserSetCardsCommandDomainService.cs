using System.Threading;
using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Domain.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSetCards.Commands;

internal sealed class UserSetCardsCommandDomainService : IUserSetCardsCommandDomainService
{
    private readonly IUserSetCardsAggregatorService _userSetCardsAggregatorService;

    public UserSetCardsCommandDomainService(ILogger logger) : this(new UserSetCardsAggregatorService(logger))
    { }

    private UserSetCardsCommandDomainService(IUserSetCardsAggregatorService userSetCardsAggregatorService) => _userSetCardsAggregatorService = userSetCardsAggregatorService;

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(
        IAddSetGroupToUserSetCardItrEntity entity,
        CancellationToken cancellationToken) => await _userSetCardsAggregatorService.AddSetGroupToUserSetCardAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddCardToSetAsync(
        IAddCardToSetItrEntity entity,
        CancellationToken cancellationToken) => await _userSetCardsAggregatorService.AddCardToSetAsync(entity, cancellationToken).ConfigureAwait(false);
}
