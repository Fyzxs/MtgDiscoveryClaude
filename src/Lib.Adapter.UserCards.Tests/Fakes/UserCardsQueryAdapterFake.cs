using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Tests.Fakes;

internal sealed class UserCardsQueryAdapterFake : IUserCardsQueryAdapter
{
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardsBySetAsyncResult { get; init; }
    public int UserCardsBySetAsyncInvokeCount { get; private set; }
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardAsyncResult { get; init; }
    public int UserCardAsyncInvokeCount { get; private set; }
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardsByIdsAsyncResult { get; init; }
    public int UserCardsByIdsAsyncInvokeCount { get; private set; }
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardsByArtistAsyncResult { get; init; }
    public int UserCardsByArtistAsyncInvokeCount { get; private set; }
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardsByNameAsyncResult { get; init; }
    public int UserCardsByNameAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(IUserCardsSetXfrEntity userCardsSet)
    {
        UserCardsBySetAsyncInvokeCount++;
        return await Task.FromResult(UserCardsBySetAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardAsync(IUserCardXfrEntity userCard)
    {
        UserCardAsyncInvokeCount++;
        return await Task.FromResult(UserCardAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByIdsAsync(IUserCardsByIdsXfrEntity userCards)
    {
        UserCardsByIdsAsyncInvokeCount++;
        return await Task.FromResult(UserCardsByIdsAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByArtistAsync(IUserCardsArtistXfrEntity userCardsArtist)
    {
        UserCardsByArtistAsyncInvokeCount++;
        return await Task.FromResult(UserCardsByArtistAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByNameAsync(IUserCardsNameXfrEntity userCardsName)
    {
        UserCardsByNameAsyncInvokeCount++;
        return await Task.FromResult(UserCardsByNameAsyncResult).ConfigureAwait(false);
    }
}
