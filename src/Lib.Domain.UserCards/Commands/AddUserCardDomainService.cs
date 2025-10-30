using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Commands;

/// <summary>
/// Single-method service for adding a user card to the collection.
/// Delegates to aggregator layer for data persistence.
/// </summary>
internal sealed class AddUserCardDomainService : IAddUserCardDomainService
{
    private readonly IUserCardsCommandAggregatorService _userCardsAggregatorService;

    public AddUserCardDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private AddUserCardDomainService(IUserCardsCommandAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardOufEntity>> Execute(IUserCardItrEntity input) => await _userCardsAggregatorService.AddUserCardAsync(input).ConfigureAwait(false);
}
