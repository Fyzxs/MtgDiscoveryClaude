using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Commands;
using Lib.MtgDiscovery.Data.Queries;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Data.Apis;

public sealed class DataService : IDataService
{
    private readonly ICardDataService _cardDataService;
    private readonly ISetDataService _setDataService;
    private readonly IArtistDataService _artistDataService;
    private readonly IUserDataService _userDataService;

    public DataService(ILogger logger) : this(new CardDataService(logger), new SetDataService(logger), new ArtistDataService(logger), new UserDataService(logger))
    { }

    private DataService(ICardDataService cardDataService, ISetDataService setDataService, IArtistDataService artistDataService, IUserDataService userDataService)
    {
        _cardDataService = cardDataService;
        _setDataService = setDataService;
        _artistDataService = artistDataService;
        _userDataService = userDataService;
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByIdsAsync(ICardIdsItrEntity args) => await _cardDataService.CardsByIdsAsync(args).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsBySetCodeAsync(ISetCodeItrEntity setCode) => await _cardDataService.CardsBySetCodeAsync(setCode).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByNameAsync(ICardNameItrEntity cardName) => await _cardDataService.CardsByNameAsync(cardName).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardNameSearchResultCollectionItrEntity>> CardNameSearchAsync(ICardSearchTermItrEntity searchTerm) => await _cardDataService.CardNameSearchAsync(searchTerm).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsAsync(ISetIdsItrEntity setIds) => await _setDataService.SetsAsync(setIds).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> SetsByCodeAsync(ISetCodesItrEntity setCodes) => await _setDataService.SetsByCodeAsync(setCodes).ConfigureAwait(false);

    public async Task<IOperationResponse<ISetItemCollectionItrEntity>> AllSetsAsync() => await _setDataService.AllSetsAsync().ConfigureAwait(false);

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermItrEntity searchTerm) => await _artistDataService.ArtistSearchAsync(searchTerm).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdItrEntity artistId) => await _artistDataService.CardsByArtistAsync(artistId).ConfigureAwait(false);

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameItrEntity artistName) => await _artistDataService.CardsByArtistNameAsync(artistName).ConfigureAwait(false);

    public async Task<IOperationResponse<IUserInfoItrEntity>> RegisterUserAsync(IUserInfoItrEntity userInfo) => await _userDataService.RegisterUserAsync(userInfo).ConfigureAwait(false);
}
