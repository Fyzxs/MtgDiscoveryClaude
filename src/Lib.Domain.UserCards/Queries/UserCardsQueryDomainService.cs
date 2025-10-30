using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
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

    public UserCardsQueryDomainService(ILogger logger) : this(
        new UserCardDomainService(logger),
        new UserCardsBySetDomainService(logger),
        new UserCardsByIdsDomainService(logger),
        new UserCardsByArtistDomainService(logger),
        new UserCardsByNameDomainService(logger))
    { }

    private UserCardsQueryDomainService(
        IUserCardDomainService userCardService,
        IUserCardsBySetDomainService userCardsBySetService,
        IUserCardsByIdsDomainService userCardsByIdsService,
        IUserCardsByArtistDomainService userCardsByArtistService,
        IUserCardsByNameDomainService userCardsByNameService)
    {
        _userCardService = userCardService;
        _userCardsBySetService = userCardsBySetService;
        _userCardsByIdsService = userCardsByIdsService;
        _userCardsByArtistService = userCardsByArtistService;
        _userCardsByNameService = userCardsByNameService;
    }

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardAsync(IUserCardItrEntity userCard) => await _userCardService.Execute(userCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsSetItrEntity userCardsSet) => await _userCardsBySetService.Execute(userCardsSet).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByIdsAsync(IUserCardsByIdsItrEntity userCards) => await _userCardsByIdsService.Execute(userCards).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByArtistAsync(IUserCardsArtistItrEntity userCardsArtist) => await _userCardsByArtistService.Execute(userCardsArtist).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsByNameAsync(IUserCardsNameItrEntity userCardsName) => await _userCardsByNameService.Execute(userCardsName).ConfigureAwait(false);
}
