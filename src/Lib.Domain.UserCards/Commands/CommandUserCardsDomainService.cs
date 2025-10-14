using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Commands;

internal sealed class CommandUserCardsDomainService : IUserCardsDomainService
{
    private readonly IUserCardsAggregatorService _userCardsAggregatorService;

    public CommandUserCardsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private CommandUserCardsDomainService(IUserCardsAggregatorService userCardsAggregatorService) => _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => await _userCardsAggregatorService.AddUserCardAsync(userCard).ConfigureAwait(false);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => throw new System.NotImplementedException("Use QueryUserCardsDomainService for read operations");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => throw new System.NotImplementedException("Use QueryUserCardsDomainService for read operations");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => throw new System.NotImplementedException("Use QueryUserCardsDomainService for read operations");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => throw new System.NotImplementedException("Use QueryUserCardsDomainService for read operations");

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => throw new System.NotImplementedException("Use QueryUserCardsDomainService for read operations");
}
