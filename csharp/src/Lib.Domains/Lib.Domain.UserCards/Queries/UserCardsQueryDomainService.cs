using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Queries;

/// <summary>
/// Query domain service for user card operations.
/// Delegates to single-method services following the Execute pattern.
/// </summary>
internal sealed class UserCardsQueryDomainService : IUserCardsQueryDomainService
{
    private readonly IUserCardDomain _userCardService;
    private readonly IUserCardsBySetDomain _userCardsBySetService;
    private readonly IUserCardsByIdsDomain _userCardsByIdsService;
    private readonly IUserCardsByArtistDomain _userCardsByArtistService;
    private readonly IUserCardsByNameDomain _userCardsByNameService;
    private readonly IUserCardsForSigningDomain _userCardsForSigningService;

    public UserCardsQueryDomainService(ILogger logger) : this(
        new UserCardDomain(logger),
        new UserCardsBySetDomain(logger),
        new UserCardsByIdsDomain(logger),
        new UserCardsByArtistDomain(logger),
        new UserCardsByNameDomain(logger),
        new UserCardsForSigningDomain(logger))
    { }

    private UserCardsQueryDomainService(
        IUserCardDomain userCardService,
        IUserCardsBySetDomain userCardsBySetService,
        IUserCardsByIdsDomain userCardsByIdsService,
        IUserCardsByArtistDomain userCardsByArtistService,
        IUserCardsByNameDomain userCardsByNameService,
        IUserCardsForSigningDomain userCardsForSigningService)
    {
        _userCardService = userCardService;
        _userCardsBySetService = userCardsBySetService;
        _userCardsByIdsService = userCardsByIdsService;
        _userCardsByArtistService = userCardsByArtistService;
        _userCardsByNameService = userCardsByNameService;
        _userCardsForSigningService = userCardsForSigningService;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(
        IUserCardItrEntity userCard,
        CancellationToken cancellationToken) => await _userCardService.Execute(userCard, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(
        IUserCardsSetItrEntity userCardsSet,
        CancellationToken cancellationToken) => await _userCardsBySetService.Execute(userCardsSet, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsItrEntity userCards,
        CancellationToken cancellationToken) => await _userCardsByIdsService.Execute(userCards, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistItrEntity userCardsArtist,
        CancellationToken cancellationToken) => await _userCardsByArtistService.Execute(userCardsArtist, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(
        IUserCardsNameItrEntity userCardsName,
        CancellationToken cancellationToken) => await _userCardsByNameService.Execute(userCardsName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<ISigningResultOufEntity>> UserCardsForSigningAsync(
        IUserCardsForSigningItrEntity userCardsForSigning,
        CancellationToken cancellationToken) => await _userCardsForSigningService.Execute(userCardsForSigning, cancellationToken).ConfigureAwait(false);
}
