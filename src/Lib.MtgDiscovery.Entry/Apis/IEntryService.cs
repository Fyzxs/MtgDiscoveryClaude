using Lib.MtgDiscovery.Entry.Commands.Collections.Apis;
using Lib.MtgDiscovery.Entry.Queries.Collections.Apis;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface IEntryService :
    ICardEntryService,
    ISetEntryService,
    IArtistEntryService,
    IUserEntryService,
    IUserCardsEntryService,
    IUserCardsQueryEntryService,
    IUserSetCardsQueryEntryService,
    IUserSetCardsCommandEntryService,
    IUserWishlistCardsEntryService,
    ISealedProductsEntryService,
    IUserSealedProductsEntryService,
    ICollectionEntryCommandService,
    ICollectionEntryQueryService;
