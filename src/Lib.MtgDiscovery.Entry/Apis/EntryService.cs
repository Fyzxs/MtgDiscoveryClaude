using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands;
using Lib.MtgDiscovery.Entry.Commands.UserSetCards;
using Lib.MtgDiscovery.Entry.Queries;
using Lib.MtgDiscovery.Entry.Queries.UserSetCards;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Outs.Sets;
using Lib.Shared.DataModels.Entities.Outs.User;
using Lib.Shared.DataModels.Entities.Outs.UserCards;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;
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
    private readonly IUserSetCardsQueryEntryService _userSetCardsQueryEntryService;
    private readonly IUserSetCardsCommandEntryService _userSetCardsCommandEntryService;

    public EntryService(ILogger logger) : this(
        new CardEntryService(logger),
        new SetEntryService(logger),
        new ArtistEntryService(logger),
        new UserEntryService(logger),
        new UserCardsEntryService(logger),
        new UserCardsQueryEntryService(logger),
        new UserSetCardsQueryEntryService(logger),
        new UserSetCardsCommandEntryService(logger))
    { }

    private EntryService(
        ICardEntryService cardEntryService,
        ISetEntryService setEntryService,
        IArtistEntryService artistEntryService,
        IUserEntryService userEntryService,
        IUserCardsEntryService userCardsEntryService,
        IUserCardsQueryEntryService userCardsQueryEntryService,
        IUserSetCardsQueryEntryService userSetCardsQueryEntryService,
        IUserSetCardsCommandEntryService userSetCardsCommandEntryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
        _artistEntryService = artistEntryService;
        _userEntryService = userEntryService;
        _userCardsEntryService = userCardsEntryService;
        _userCardsQueryEntryService = userCardsQueryEntryService;
        _userSetCardsQueryEntryService = userSetCardsQueryEntryService;
        _userSetCardsCommandEntryService = userSetCardsCommandEntryService;
    }

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args) => _cardEntryService.CardsByIdsAsync(args);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(ISetCodeArgEntity setCode) => _cardEntryService.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(ICardNameArgEntity cardName) => _cardEntryService.CardsByNameAsync(cardName);

    public Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm) => _cardEntryService.CardNameSearchAsync(searchTerm);

    public Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity setIds) => _setEntryService.SetsByIdsAsync(setIds);

    public Task<IOperationResponse<List<ScryfallSetOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity setCodes) => _setEntryService.SetsByCodeAsync(setCodes);

    public Task<IOperationResponse<List<ScryfallSetOutEntity>>> AllSetsAsync() => _setEntryService.AllSetsAsync();

    public Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm) => _artistEntryService.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId) => _artistEntryService.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(IArtistNameArgEntity artistName) => _artistEntryService.CardsByArtistNameAsync(artistName);

    public Task<IOperationResponse<UserRegistrationOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser) => _userEntryService.RegisterUserAsync(authUser);

    public Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAuthUserArgEntity authUser, IAddUserCardArgEntity args) => _userCardsEntryService.AddCardToCollectionAsync(authUser, args);

    public Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs) => _userCardsQueryEntryService.UserCardAsync(cardArgs);

    public Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs) => _userCardsQueryEntryService.UserCardsBySetAsync(bySetArgs);

    public Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs) => _userCardsQueryEntryService.UserCardsByIdsAsync(cardsArgs);

    public Task<IOperationResponse<UserSetCardOutEntity>> GetUserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs) => _userSetCardsQueryEntryService.GetUserSetCardByUserAndSetAsync(userSetCardArgs);

    public Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAuthUserArgEntity authUser, IAddSetGroupToUserSetCardArgEntity argEntity) => _userSetCardsCommandEntryService.AddSetGroupToUserSetCardAsync(authUser, argEntity);
}
