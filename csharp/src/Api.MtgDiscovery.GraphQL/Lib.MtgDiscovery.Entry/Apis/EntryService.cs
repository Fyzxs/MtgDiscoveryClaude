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

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(
        ICardIdsArgEntity args,
        CancellationToken cancellationToken)
        => await _cardEntryService.CardsByIdsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(
        ISetCodeArgEntity setCode,
        CancellationToken cancellationToken)
        => await _cardEntryService.CardsBySetCodeAsync(setCode, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(
        ICardNameArgEntity cardName,
        CancellationToken cancellationToken)
        => await _cardEntryService.CardsByNameAsync(cardName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(
        ICardSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken)
        => await _cardEntryService.CardNameSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<SetItemOutEntity>>> SetsByIdsAsync(
        ISetIdsArgEntity setIds,
        CancellationToken cancellationToken)
        => await _setEntryService.SetsByIdsAsync(setIds, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<SetItemOutEntity>>> SetsByCodeAsync(
        ISetCodesArgEntity setCodes,
        CancellationToken cancellationToken)
        => await _setEntryService.SetsByCodeAsync(setCodes, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<SetItemOutEntity>>> AllSetsAsync(
        IAllSetsArgEntity args,
        CancellationToken cancellationToken)
        => await _setEntryService.AllSetsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(
        IArtistSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken)
        => await _artistEntryService.ArtistSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(
        IArtistIdArgEntity artistId,
        CancellationToken cancellationToken)
        => await _artistEntryService.CardsByArtistAsync(artistId, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(
        IArtistNameArgEntity artistName,
        CancellationToken cancellationToken)
        => await _artistEntryService.CardsByArtistNameAsync(artistName, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSyncOutEntity>> RegisterUserAsync(IAuthUserArgEntity authUser, CancellationToken cancellationToken) => await _userEntryService.RegisterUserAsync(authUser, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args, CancellationToken cancellationToken) => await _userCardsEntryService.AddCardToCollectionAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddUserCardOnlyAsync(IAddCardToCollectionArgsEntity args, CancellationToken cancellationToken) => await _userCardsEntryService.AddUserCardOnlyAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardAsync(IUserCardArgEntity cardArgs, CancellationToken cancellationToken) => await _userCardsQueryEntryService.UserCardAsync(cardArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsBySetAsync(IUserCardsBySetArgEntity bySetArgs, CancellationToken cancellationToken) => await _userCardsQueryEntryService.UserCardsBySetAsync(bySetArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserCardOutEntity>>> UserCardsByIdsAsync(IUserCardsByIdsArgEntity cardsArgs, CancellationToken cancellationToken) => await _userCardsQueryEntryService.UserCardsByIdsAsync(cardsArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<SigningResultOutEntity>> UserCardsForSigningAsync(IUserCardsForSigningArgEntity forSigningArgs, CancellationToken cancellationToken) => await _userCardsQueryEntryService.UserCardsForSigningAsync(forSigningArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardOutEntity>> UserSetCardByUserAndSetAsync(IUserSetCardArgEntity userSetCardArgs, CancellationToken cancellationToken) => await _userSetCardsQueryEntryService.UserSetCardByUserAndSetAsync(userSetCardArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserSetCardOutEntity>>> AllUserSetCardsAsync(IAllUserSetCardsArgEntity userSetCardsArgs, CancellationToken cancellationToken) => await _userSetCardsQueryEntryService.AllUserSetCardsAsync(userSetCardsArgs, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddSetGroupToUserSetCardAsync(IAddSetGroupToUserSetCardArgsEntity args, CancellationToken cancellationToken) => await _userSetCardsCommandEntryService.AddSetGroupToUserSetCardAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<UserSetCardOutEntity>> AddCardToSetAsync(IAddCardToSetArgsEntity args, CancellationToken cancellationToken) => await _userSetCardsCommandEntryService.AddCardToSetAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToWishlistAsync(IAddCardToWishlistArgsEntity args, CancellationToken cancellationToken) => await _userWishlistCardsEntryService.AddCardToWishlistAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CardItemOutEntity>>> GetUserWishlistAsync(IGetUserWishlistArgsEntity args, CancellationToken cancellationToken) => await _userWishlistCardsEntryService.GetUserWishlistAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> SealedProductsBySetCodeAsync(
        ISealedProductsBySetCodeArgEntity args,
        CancellationToken cancellationToken) => await _sealedProductsEntryService.SealedProductsBySetCodeAsync(args, cancellationToken);

    public async Task<IOperationResponse<List<SealedProductOutEntity>>> AddSealedProductToCollectionAsync(IAddSealedProductToCollectionArgsEntity args, CancellationToken cancellationToken) => await _userSealedProductsEntryService.AddSealedProductToCollectionAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<UserSealedProductOutEntity>>> GetUserSealedProductsByUserIdAsync(string userId, CancellationToken cancellationToken) => await _userSealedProductsEntryService.GetUserSealedProductsByUserIdAsync(userId, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> CreateCollectionAsync(ICreateCollectionArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.CreateCollectionAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> RenameCollectionAsync(IRenameCollectionArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.RenameCollectionAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> UpdateCollectionVisibilityAsync(IUpdateCollectionVisibilityArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.UpdateCollectionVisibilityAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> GrantCollectionAccessAsync(IGrantCollectionAccessArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.GrantCollectionAccessAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> RevokeCollectionAccessAsync(IRevokeCollectionAccessArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.RevokeCollectionAccessAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> DeleteCollectionAsync(IDeleteCollectionArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.DeleteCollectionAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> TransferCollectionOwnershipAsync(ITransferCollectionOwnershipArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.TransferCollectionOwnershipAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<AuthorizedUserOutEntity>>> GetCollectionAccessListAsync(IGetCollectionAccessListArgsEntity argsEntity, CancellationToken cancellationToken) => await _collectionEntryCommandService.GetCollectionAccessListAsync(argsEntity, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CollectionOutEntity>>> MyCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.MyCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<CollectionOutEntity>> GetCollectionByIdAsync(ICollectionIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.GetCollectionByIdAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CollectionOutEntity>>> SharedCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.SharedCollectionsAsync(args, cancellationToken).ConfigureAwait(false);

    public async Task<IOperationResponse<List<CollectionOutEntity>>> AccessibleCollectionsAsync(IUserIdArgEntity args, CancellationToken cancellationToken) => await _collectionEntryQueryService.AccessibleCollectionsAsync(args, cancellationToken).ConfigureAwait(false);
}
