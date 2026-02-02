using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Cosmos DB implementation of the user cards query adapter.
///
/// This class coordinates all Cosmos DB-specific user cards query operations
/// by delegating to specialized single-method adapters.
/// The main UserCardsAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserCardsQueryAdapter : IUserCardsQueryAdapter
{
    private readonly IUserCardsBySetAdapter _userCardsBySetAdapter;
    private readonly IUserCardAdapter _userCardAdapter;
    private readonly IUserCardsByIdsAdapter _userCardsByIdsAdapter;
    private readonly IUserCardsByArtistAdapter _userCardsByArtistAdapter;
    private readonly IUserCardsByNameAdapter _userCardsByNameAdapter;
    private readonly IUserCardsForSigningAdapter _userCardsForSigningAdapter;

    public UserCardsQueryAdapter(ILogger logger) : this(new UserCardsBySetAdapter(logger), new UserCardAdapter(logger), new UserCardsByIdsAdapter(logger), new UserCardsByArtistAdapter(logger), new UserCardsByNameAdapter(logger), new UserCardsForSigningAdapter(logger)) { }

    private UserCardsQueryAdapter(IUserCardsBySetAdapter userCardsBySetAdapter, IUserCardAdapter userCardAdapter, IUserCardsByIdsAdapter userCardsByIdsAdapter, IUserCardsByArtistAdapter userCardsByArtistAdapter, IUserCardsByNameAdapter userCardsByNameAdapter, IUserCardsForSigningAdapter userCardsForSigningAdapter)
    {
        _userCardsBySetAdapter = userCardsBySetAdapter;
        _userCardAdapter = userCardAdapter;
        _userCardsByIdsAdapter = userCardsByIdsAdapter;
        _userCardsByArtistAdapter = userCardsByArtistAdapter;
        _userCardsByNameAdapter = userCardsByNameAdapter;
        _userCardsForSigningAdapter = userCardsForSigningAdapter;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(IUserCardsSetXfrEntity userCardsSet) => await _userCardsBySetAdapter.Execute(userCardsSet).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardAsync(IUserCardXfrEntity userCard) => await _userCardAdapter.Execute(userCard).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByIdsAsync(IUserCardsByIdsXfrEntity userCards) => await _userCardsByIdsAdapter.Execute(userCards).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByArtistAsync(IUserCardsArtistXfrEntity userCardsArtist) => await _userCardsByArtistAdapter.Execute(userCardsArtist).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByNameAsync(IUserCardsNameXfrEntity userCardsName) => await _userCardsByNameAdapter.Execute(userCardsName).ConfigureAwait(false);

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsForSigningAsync(IUserCardsForSigningXfrEntity userCardsForSigning) => await _userCardsForSigningAdapter.Execute(userCardsForSigning).ConfigureAwait(false);
}
