namespace Lib.MtgDiscovery.Entry.Apis;

public interface IEntryService :
    ICardEntryService,
    ISetEntryService,
    IArtistEntryService,
    IUserEntryService,
    IUserCardsEntryService,
    IUserCardsQueryEntryService,
    IUserSetCardsQueryEntryService;
