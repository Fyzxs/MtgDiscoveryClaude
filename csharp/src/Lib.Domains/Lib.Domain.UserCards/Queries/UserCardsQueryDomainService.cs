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
    private readonly IUserCardDomainService _userCardService;
    private readonly IUserCardsBySetDomainService _userCardsBySetService;
    private readonly IUserCardsByIdsDomainService _userCardsByIdsService;
    private readonly IUserCardsByArtistDomainService _userCardsByArtistService;
    private readonly IUserCardsByNameDomainService _userCardsByNameService;
    private readonly IUserCardsForSigningDomainService _userCardsForSigningService;

    public UserCardsQueryDomainService(ILogger logger) : this(
        new UserCardDomainService(logger),
        new UserCardsBySetDomainService(logger),
        new UserCardsByIdsDomainService(logger),
        new UserCardsByArtistDomainService(logger),
        new UserCardsByNameDomainService(logger),
        new UserCardsForSigningDomainService(logger))
    { }

    private UserCardsQueryDomainService(
        IUserCardDomainService userCardService,
        IUserCardsBySetDomainService userCardsBySetService,
        IUserCardsByIdsDomainService userCardsByIdsService,
        IUserCardsByArtistDomainService userCardsByArtistService,
        IUserCardsByNameDomainService userCardsByNameService,
        IUserCardsForSigningDomainService userCardsForSigningService)
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
