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
    private readonly IUserCardsQueryDomainService _queryService;
    private readonly IUserCardsCommandDomainService _commandService;

    public UserCardsDomainService(ILogger logger) : this(new UserCardsQueryDomainService(logger), new UserCardsCommandDomainService(logger))
    { }

    private UserCardsDomainService(IUserCardsQueryDomainService queryService, IUserCardsCommandDomainService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _commandService.AddUserCardAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserCardOufEntity>> AddUserCardOnlyAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _commandService.AddUserCardOnlyAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _queryService.UserCardAsync(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(
        IUserCardsSetItrEntity userCardsSet,
        CancellationToken cancellationToken) => await _queryService.UserCardsBySetAsync(userCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsItrEntity userCards,
        CancellationToken cancellationToken) => await _queryService.UserCardsByIdsAsync(userCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistItrEntity userCardsArtist,
        CancellationToken cancellationToken) => await _queryService.UserCardsByArtistAsync(userCardsArtist, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(
        IUserCardsNameItrEntity userCardsName,
        CancellationToken cancellationToken) => await _queryService.UserCardsByNameAsync(userCardsName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(
        IUserCardsForSigningItrEntity userCardsForSigning,
        CancellationToken cancellationToken) => await _queryService.UserCardsForSigningAsync(userCardsForSigning, cancellationToken).ConfigureAwait(false);
}
