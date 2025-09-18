using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands;
using Lib.MtgDiscovery.Entry.Queries;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

public sealed class EntryService : IEntryService
{
    private readonly ICardEntryService _cardEntryService;
    private readonly ISetEntryService _setEntryService;
    private readonly IArtistEntryService _artistEntryService;
    private readonly IUserEntryService _userEntryService;
    private readonly IUserCardsEntryService _userCardsEntryService;
    private readonly IUserCardsQueryEntryService _userCardsQueryEntryService;

    public EntryService(ILogger logger) : this(
        new CardEntryService(logger),
        new SetEntryService(logger),
        new ArtistEntryService(logger),
        new UserEntryService(logger),
        new UserCardsEntryService(logger),
        new UserCardsQueryEntryService(logger))
    { }

    private EntryService(
        ICardEntryService cardEntryService,
        ISetEntryService setEntryService,
        IArtistEntryService artistEntryService,
        IUserEntryService userEntryService,
        IUserCardsEntryService userCardsEntryService,
        IUserCardsQueryEntryService userCardsQueryEntryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
        _artistEntryService = artistEntryService;
        _userEntryService = userEntryService;
        _userCardsEntryService = userCardsEntryService;
        _userCardsQueryEntryService = userCardsQueryEntryService;
    }

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(ICardIdsArgEntity args) => _cardEntryService.CardsByIdsAsync(args);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode) => _cardEntryService.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(ICardNameArgEntity cardName) => _cardEntryService.CardsByNameAsync(cardName);

    public Task<IOperationResponse<ICardNameSearchResultCollectionOufEntity>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm) => _cardEntryService.CardNameSearchAsync(searchTerm);

    public Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByIdsAsync(ISetIdsArgEntity setIds) => _setEntryService.SetsByIdsAsync(setIds);

    public Task<IOperationResponse<ISetItemCollectionOufEntity>> SetsByCodeAsync(ISetCodesArgEntity setCodes) => _setEntryService.SetsByCodeAsync(setCodes);

    public Task<IOperationResponse<ISetItemCollectionOufEntity>> AllSetsAsync() => _setEntryService.AllSetsAsync();

    public Task<IOperationResponse<IArtistSearchResultCollectionOufEntity>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm) => _artistEntryService.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistAsync(IArtistIdArgEntity artistId) => _artistEntryService.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByArtistNameAsync(IArtistNameArgEntity artistName) => _artistEntryService.CardsByArtistNameAsync(artistName);

    public Task<IOperationResponse<IUserInfoOufEntity>> RegisterUserAsync(IAuthUserArgEntity authUser) => _userEntryService.RegisterUserAsync(authUser);

    public Task<IOperationResponse<IUserCardOufEntity>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddUserCardArgEntity args) => _userCardsEntryService.AddCardToCollectionAsync(authUser, args);

    public Task<IOperationResponse<IEnumerable<IUserCardOufEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs) => _userCardsQueryEntryService.UserCardsBySetAsync(bySetArgs);
}
