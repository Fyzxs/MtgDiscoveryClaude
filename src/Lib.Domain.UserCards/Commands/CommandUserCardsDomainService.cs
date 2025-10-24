using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Commands;

internal sealed class CommandUserCardsDomainService : IUserCardsCommandDomainService
{
    private readonly IUserCardsCommandAggregatorService _userCardsAggregatorService;

    public CommandUserCardsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private CommandUserCardsDomainService(IUserCardsCommandAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => await _userCardsAggregatorService.AddUserCardAsync(userCard).ConfigureAwait(false);
}
