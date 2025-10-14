using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Commands;
using Lib.Domain.UserCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Apis;

public sealed class UserCardsDomainService : IUserCardsDomainService
{
    private readonly IUserCardsDomainService _queryOperations;
    private readonly IUserCardsDomainService _commandOperations;

    public UserCardsDomainService(ILogger logger) : this(new QueryUserCardsDomainService(logger), new CommandUserCardsDomainService(logger))
    { }

    private UserCardsDomainService(IUserCardsDomainService queryOperations, IUserCardsDomainService commandOperations)
    {
        _queryOperations = queryOperations;
        _commandOperations = commandOperations;
    }

    public Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => _commandOperations.AddUserCardAsync(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => _queryOperations.UserCardAsync(userCard);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => _queryOperations.UserCardsBySetAsync(userCardsSet);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => _queryOperations.UserCardsByIdsAsync(userCards);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => _queryOperations.UserCardsByArtistAsync(userCardsArtist);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => _queryOperations.UserCardsByNameAsync(userCardsName);
}
