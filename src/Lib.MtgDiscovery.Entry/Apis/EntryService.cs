using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

public sealed class EntryService : IEntryService
{
    private readonly ICardEntryService _cardEntryService;
    private readonly ISetEntryService _setEntryService;
    private readonly IArtistEntryService _artistEntryService;
    private readonly IUserEntryService _userEntryService;

    public EntryService(ILogger logger) : this(new CardEntryService(logger), new SetEntryService(logger), new ArtistEntryService(logger), new UserEntryService(logger))
    { }

    private EntryService(ICardEntryService cardEntryService, ISetEntryService setEntryService, IArtistEntryService artistEntryService, IUserEntryService userEntryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
        _artistEntryService = artistEntryService;
        _userEntryService = userEntryService;
    }

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsArgEntity args) => _cardEntryService.CardsByIdsAsync(args);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeArgEntity setCode) => _cardEntryService.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameArgEntity cardName) => _cardEntryService.CardsByNameAsync(cardName);

    public Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm) => _cardEntryService.CardNameSearchAsync(searchTerm);

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByIdsAsync(ISetIdsArgEntity setIds) => _setEntryService.SetsByIdsAsync(setIds);

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesArgEntity setCodes) => _setEntryService.SetsByCodeAsync(setCodes);

    public Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync() => _setEntryService.AllSetsAsync();

    public Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm) => _artistEntryService.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdArgEntity artistId) => _artistEntryService.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameArgEntity artistName) => _artistEntryService.CardsByArtistNameAsync(artistName);

    public Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IAuthUserArgEntity authUser) => _userEntryService.RegisterUserAsync(authUser);
}
