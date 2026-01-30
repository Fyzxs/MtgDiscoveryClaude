using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands;
using Lib.MtgDiscovery.Entry.Commands.Collections;
using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.MtgDiscovery.Entry.Commands.UserSetCards;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.MtgDiscovery.Entry.Entities.Outs.Collections;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.Sets;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.MtgDiscovery.Entry.Entities.Outs.User;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserCards;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSealedProducts;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.MtgDiscovery.Entry.Queries;
using Lib.MtgDiscovery.Entry.Queries.Collections;
using Lib.MtgDiscovery.Entry.Queries.Collections.Apis;
using Lib.MtgDiscovery.Entry.Queries.UserSetCards;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Args.Cards;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Args.User;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Args.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
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
    private readonly IUserWishlistCardsEntryService _userWishlistCardsEntryService;
    private readonly ISealedProductsEntryService _sealedProductsEntryService;
    private readonly IUserSealedProductsEntryService _userSealedProductsEntryService;
    private readonly ICollectionEntryCommandService _collectionEntryCommandService;
    private readonly ICollectionEntryQueryService _collectionEntryQueryService;

    public EntryService(ILogger logger) : this(
        new CardEntryService(logger),
        new SetEntryService(logger),
        new ArtistEntryService(logger),
        new UserEntryService(logger),
        new UserCardsEntryService(logger),
        new UserCardsQueryEntryService(logger),
        new UserSetCardsQueryEntryService(logger),
        new UserSetCardsCommandEntryService(logger),
        new UserWishlistCardsEntryService(logger),
        new SealedProductsEntryService(logger),
        new UserSealedProductsEntryService(logger),
        new CollectionEntryCommandService(logger),
        new CollectionEntryQueryService(logger))
    { }

    private EntryService(
        ICardEntryService cardEntryService,
        ISetEntryService setEntryService,
        IArtistEntryService artistEntryService,
        IUserEntryService userEntryService,
        IUserCardsEntryService userCardsEntryService,
        IUserCardsQueryEntryService userCardsQueryEntryService,
        IUserSetCardsQueryEntryService userSetCardsQueryEntryService,
        IUserSetCardsCommandEntryService userSetCardsCommandEntryService,
        IUserWishlistCardsEntryService userWishlistCardsEntryService,
        ISealedProductsEntryService sealedProductsEntryService,
        IUserSealedProductsEntryService userSealedProductsEntryService,
        ICollectionEntryCommandService collectionEntryCommandService,
        ICollectionEntryQueryService collectionEntryQueryService)
    {
        _cardEntryService = cardEntryService;
        _setEntryService = setEntryService;
        _artistEntryService = artistEntryService;
        _userEntryService = userEntryService;
        _userCardsEntryService = userCardsEntryService;
        _userCardsQueryEntryService = userCardsQueryEntryService;
        _userSetCardsQueryEntryService = userSetCardsQueryEntryService;
        _userSetCardsCommandEntryService = userSetCardsCommandEntryService;
        _userWishlistCardsEntryService = userWishlistCardsEntryService;
        _sealedProductsEntryService = sealedProductsEntryService;
        _userSealedProductsEntryService = userSealedProductsEntryService;
        _collectionEntryCommandService = collectionEntryCommandService;
        _collectionEntryQueryService = collectionEntryQueryService;
    }

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args) => _cardEntryService.CardsByIdsAsync(args);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(ISetCodeArgEntity setCode) => _cardEntryService.CardsBySetCodeAsync(setCode);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(ICardNameArgEntity cardName) => _cardEntryService.CardsByNameAsync(cardName);

    public Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm) => _cardEntryService.CardNameSearchAsync(searchTerm);

    public Task<IOperationResponse<List<SetItemOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity setIds) => _setEntryService.SetsByIdsAsync(setIds);

    public Task<IOperationResponse<List<SetItemOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity setCodes) => _setEntryService.SetsByCodeAsync(setCodes);

    public Task<IOperationResponse<List<SetItemOutEntity>>> AllSetsAsync(IAllSetsArgEntity args) => _setEntryService.AllSetsAsync(args);

    public Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm) => _artistEntryService.ArtistSearchAsync(searchTerm);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId) => _artistEntryService.CardsByArtistAsync(artistId);

    public Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(IArtistNameArgEntity artistName) => _artistEntryService.CardsByArtistNameAsync(artistName);

    public Task<IOperationResponse<UserSyncOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser) => _userEntryService.RegisterUserAsync(authUser);

    public Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args) => _userCardsEntryService.AddCardToCollectionAsync(args);

    public Task<IOperationResponse<List<CardItemOutEntity>>> AddUserCardOnlyAsync(IAddCardToCollectionArgsEntity args) => _userCardsEntryService.AddUserCardOnlyAsync(args);

    public Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs) => _userCardsQueryEntryService.UserCardAsync(cardArgs);

    public Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs) => _userCardsQueryEntryService.UserCardsBySetAsync(bySetArgs);

    public Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs) => _userCardsQueryEntryService.UserCardsByIdsAsync(cardsArgs);

    public Task<IOperationResponse<SigningResultOutEntity>> UserCardsForSigningAsync(IUserCardsForSigningArgEntity forSigningArgs) => _userCardsQueryEntryService.UserCardsForSigningAsync(forSigningArgs);

    public Task<IOperationResponse<UserSetCardOutEntity>> UserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs) => _userSetCardsQueryEntryService.UserSetCardByUserAndSetAsync(userSetCardArgs);

    public Task<IOperationResponse<List<UserSetCardOutEntity>>> AllUserSetCardsAsync(IAllUserSetCardsArgEntity userSetCardsArgs) => _userSetCardsQueryEntryService.AllUserSetCardsAsync(userSetCardsArgs);

    public Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardArgsEntity args) => _userSetCardsCommandEntryService.AddSetGroupToUserSetCardAsync(args);

    public Task<IOperationResponse<UserSetCardOutEntity>> AddCardToSetAsync(IAddCardToSetArgsEntity args) => _userSetCardsCommandEntryService.AddCardToSetAsync(args);

    public Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToWishlistAsync(IAddCardToWishlistArgsEntity args) => _userWishlistCardsEntryService.AddCardToWishlistAsync(args);

    public Task<IOperationResponse<List<CardItemOutEntity>>> GetUserWishlistAsync(IGetUserWishlistArgsEntity args) => _userWishlistCardsEntryService.GetUserWishlistAsync(args);

    public Task<IOperationResponse<List<SealedProductOutEntity>>> SealedProductsBySetCodeAsync(ISealedProductsBySetCodeArgEntity args) => _sealedProductsEntryService.SealedProductsBySetCodeAsync(args);

    public Task<IOperationResponse<List<SealedProductOutEntity>>> AddSealedProductToCollectionAsync(IAddSealedProductToCollectionArgsEntity args) => _userSealedProductsEntryService.AddSealedProductToCollectionAsync(args);

    public Task<IOperationResponse<List<UserSealedProductOutEntity>>> GetUserSealedProductsByUserIdAsync(string userId) => _userSealedProductsEntryService.GetUserSealedProductsByUserIdAsync(userId);

    public Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgsEntity argsEntity) => _collectionEntryCommandService.CreateCollectionAsync(argsEntity);

    public Task<IOperationResponse<CollectionOutEntity>> RenameCollectionAsync(IRenameCollectionArgsEntity argsEntity) => _collectionEntryCommandService.RenameCollectionAsync(argsEntity);

    public Task<IOperationResponse<CollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgsEntity argsEntity) => _collectionEntryCommandService.UpdateCollectionVisibilityAsync(argsEntity);

    public Task<IOperationResponse<CollectionOutEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessArgsEntity argsEntity) => _collectionEntryCommandService.GrantCollectionAccessAsync(argsEntity);

    public Task<IOperationResponse<CollectionOutEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessArgsEntity argsEntity) => _collectionEntryCommandService.RevokeCollectionAccessAsync(argsEntity);

    public Task<IOperationResponse<CollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgsEntity argsEntity) => _collectionEntryCommandService.DeleteCollectionAsync(argsEntity);

    public Task<IOperationResponse<CollectionOutEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipArgsEntity argsEntity) => _collectionEntryCommandService.TransferCollectionOwnershipAsync(argsEntity);

    public Task<IOperationResponse<IEnumerable<AuthorizedUserOutEntity>>> GetCollectionAccessListAsync(IGetCollectionAccessListArgsEntity argsEntity) => _collectionEntryCommandService.GetCollectionAccessListAsync(argsEntity);

    public Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(string userId) => _collectionEntryQueryService.MyCollectionsAsync(userId);

    public Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(string collectionId, string userId) => _collectionEntryQueryService.GetCollectionByIdAsync(collectionId, userId);

    public Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(string userId) => _collectionEntryQueryService.SharedCollectionsAsync(userId);

    public Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(string userId) => _collectionEntryQueryService.AccessibleCollectionsAsync(userId);
}
