using System.Collections.Generic;
using System.Threading;
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

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _commandOperations.AddUserCardAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _commandOperations.AddUserCardOnlyAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _queryOperations.UserCardAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(
        IUserCardsSetItrEntity userCardsSet,
        CancellationToken cancellationToken) => await _queryOperations.UserCardsBySetAsync(userCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsItrEntity userCards,
        CancellationToken cancellationToken) => await _queryOperations.UserCardsByIdsAsync(userCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistItrEntity userCardsArtist,
        CancellationToken cancellationToken) => await _queryOperations.UserCardsByArtistAsync(userCardsArtist, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(
        IUserCardsNameItrEntity userCardsName,
        CancellationToken cancellationToken) => await _queryOperations.UserCardsByNameAsync(userCardsName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(
        IUserCardsForSigningItrEntity userCardsForSigning,
        CancellationToken cancellationToken) => await _queryOperations.UserCardsForSigningAsync(userCardsForSigning, cancellationToken).ConfigureAwait(false);
}
