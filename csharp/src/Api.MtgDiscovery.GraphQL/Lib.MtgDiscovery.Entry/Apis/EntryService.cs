using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using Lib.Shared.DataModels.Entities.Args.Collections;
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

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(ICardIdsArgEntity args) => await _cardEntryService.CardsByIdsAsync(args);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(ISetCodeArgEntity setCode) => await _cardEntryService.CardsBySetCodeAsync(setCode);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(ICardNameArgEntity cardName) => await _cardEntryService.CardsByNameAsync(cardName);

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(ICardSearchTermArgEntity searchTerm) => await _cardEntryService.CardNameSearchAsync(searchTerm);

    public async Task<IOperationResponse<List<SetItemOutEntity>>> SetsByIdsAsync(ISetIdsArgEntity setIds) => await _setEntryService.SetsByIdsAsync(setIds);

    public async Task<IOperationResponse<List<SetItemOutEntity>>> SetsByCodeAsync(ISetCodesArgEntity setCodes) => await _setEntryService.SetsByCodeAsync(setCodes);

    public async Task<IOperationResponse<List<SetItemOutEntity>>> AllSetsAsync(IAllSetsArgEntity args) => await _setEntryService.AllSetsAsync(args);

    public async Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm) => await _artistEntryService.ArtistSearchAsync(searchTerm);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId) => await _artistEntryService.CardsByArtistAsync(artistId);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(IArtistNameArgEntity artistName) => await _artistEntryService.CardsByArtistNameAsync(artistName);

    public async Task<IOperationResponse<UserSyncOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser) => await _userEntryService.RegisterUserAsync(authUser);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args) => await _userCardsEntryService.AddCardToCollectionAsync(args);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddUserCardOnlyAsync(IAddCardToCollectionArgsEntity args) => await _userCardsEntryService.AddUserCardOnlyAsync(args);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs) => await _userCardsQueryEntryService.UserCardAsync(cardArgs);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs) => await _userCardsQueryEntryService.UserCardsBySetAsync(bySetArgs);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs) => await _userCardsQueryEntryService.UserCardsByIdsAsync(cardsArgs);

    public async Task<IOperationResponse<SigningResultOutEntity>> UserCardsForSigningAsync(IUserCardsForSigningArgEntity forSigningArgs) => await _userCardsQueryEntryService.UserCardsForSigningAsync(forSigningArgs);

    public async Task<IOperationResponse<UserSetCardOutEntity>> UserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs) => await _userSetCardsQueryEntryService.UserSetCardByUserAndSetAsync(userSetCardArgs);

    public async Task<IOperationResponse<List<UserSetCardOutEntity>>> AllUserSetCardsAsync(IAllUserSetCardsArgEntity userSetCardsArgs) => await _userSetCardsQueryEntryService.AllUserSetCardsAsync(userSetCardsArgs);

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardArgsEntity args) => await _userSetCardsCommandEntryService.AddSetGroupToUserSetCardAsync(args);

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddCardToSetAsync(IAddCardToSetArgsEntity args) => await _userSetCardsCommandEntryService.AddCardToSetAsync(args);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToWishlistAsync(IAddCardToWishlistArgsEntity args) => await _userWishlistCardsEntryService.AddCardToWishlistAsync(args);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> GetUserWishlistAsync(IGetUserWishlistArgsEntity args) => await _userWishlistCardsEntryService.GetUserWishlistAsync(args);

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken) => await _sealedProductsEntryService.SealedProductsBySetCodeAsync(args, cancellationToken);

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> AddSealedProductToCollectionAsync(IAddSealedProductToCollectionArgsEntity args) => await _userSealedProductsEntryService.AddSealedProductToCollectionAsync(args);

    public async Task<IOperationResponse<List<UserSealedProductOutEntity>>> GetUserSealedProductsByUserIdAsync(string userId) => await _userSealedProductsEntryService.GetUserSealedProductsByUserIdAsync(userId);

    public async Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgsEntity argsEntity) => await _collectionEntryCommandService.CreateCollectionAsync(argsEntity);

    public async Task<IOperationResponse<CollectionOutEntity>> RenameCollectionAsync(IRenameCollectionArgsEntity argsEntity) => await _collectionEntryCommandService.RenameCollectionAsync(argsEntity);

    public async Task<IOperationResponse<CollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgsEntity argsEntity) => await _collectionEntryCommandService.UpdateCollectionVisibilityAsync(argsEntity);

    public async Task<IOperationResponse<CollectionOutEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessArgsEntity argsEntity) => await _collectionEntryCommandService.GrantCollectionAccessAsync(argsEntity);

    public async Task<IOperationResponse<CollectionOutEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessArgsEntity argsEntity) => await _collectionEntryCommandService.RevokeCollectionAccessAsync(argsEntity);

    public async Task<IOperationResponse<CollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgsEntity argsEntity) => await _collectionEntryCommandService.DeleteCollectionAsync(argsEntity);

    public async Task<IOperationResponse<CollectionOutEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipArgsEntity argsEntity) => await _collectionEntryCommandService.TransferCollectionOwnershipAsync(argsEntity);

    public async Task<IOperationResponse<IEnumerable<AuthorizedUserOutEntity>>> GetCollectionAccessListAsync(IGetCollectionAccessListArgsEntity argsEntity) => await _collectionEntryCommandService.GetCollectionAccessListAsync(argsEntity);

    public async Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.MyCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(ICollectionIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.GetCollectionByIdAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.SharedCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.AccessibleCollectionsAsync(args, cancellationToken).ConfigureAwait(false);
}
