using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Commands;
using Lib.Domain.UserCards.Queries;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Apis;

public sealed class UserCardsDomainService : IUserCardsDomainService
{
    private readonly IUserCardsQueryDomainService _queryOperations;
    private readonly IUserCardsCommandDomainService _commandOperations;

    public UserCardsDomainService(ILogger logger) : this(new UserCardsQueryDomainService(logger), new UserCardsCommandDomainService(logger))
    { }

    private UserCardsDomainService(IUserCardsQueryDomainService queryOperations, IUserCardsCommandDomainService commandOperations)
    {
        _queryOperations = queryOperations;
        _commandOperations = commandOperations;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(IUserCardItrEntity userCard) => await _commandOperations.AddUserCardAsync(userCard);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(IUserCardItrEntity userCard) => await _commandOperations.AddUserCardOnlyAsync(userCard);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => await _queryOperations.UserCardAsync(userCard);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => await _queryOperations.UserCardsBySetAsync(userCardsSet);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => await _queryOperations.UserCardsByIdsAsync(userCards);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => await _queryOperations.UserCardsByArtistAsync(userCardsArtist);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => await _queryOperations.UserCardsByNameAsync(userCardsName);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(IUserCardsForSigningItrEntity userCardsForSigning) => await _queryOperations.UserCardsForSigningAsync(userCardsForSigning);
}
