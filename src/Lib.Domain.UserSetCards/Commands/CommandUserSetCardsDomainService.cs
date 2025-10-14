using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Apis;
using Lib.Domain.UserSetCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserSetCards.Commands;

internal sealed class CommandUserSetCardsDomainService : IUserSetCardsDomainService
{
    private readonly IUserSetCardsAggregatorService _userSetCardsAggregatorService;

    public CommandUserSetCardsDomainService(ILogger logger) : this(new UserSetCardsAggregatorService(logger))
    { }

    private CommandUserSetCardsDomainService(IUserSetCardsAggregatorService userSetCardsAggregatorService) => _userSetCardsAggregatorService = userSetCardsAggregatorService;

    public Task<IOperationResponse<IUserSetCardOufEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardItrEntity userSetCard) => throw new System.NotImplementedException("Use QueryUserSetCardsDomainService for read operations");

    public async Task<IOperationResponse<IUserSetCardOufEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardItrEntity entity) => await _userSetCardsAggregatorService.AddSetGroupToUserSetCardAsync(entity).ConfigureAwait(false);
}
