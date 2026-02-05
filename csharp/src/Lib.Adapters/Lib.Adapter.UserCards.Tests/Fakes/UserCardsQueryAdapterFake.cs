using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.UserCards.Tests.Fakes;

public sealed class UserCardsQueryAdapterFake : IUserCardsQueryAdapter
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
    public IOperationResponse<IEnumerable<UserCardExtEntity>> UserCardsForSigningAsyncResult { get; init; }
    public int UserCardsForSigningAsyncInvokeCount { get; private set; }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(
        IUserCardsSetXfrEntity userCardsSet,
        CancellationToken cancellationToken)
    {
        UserCardsBySetAsyncInvokeCount++;
        return await Task.FromResult(UserCardsBySetAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardAsync(
        IUserCardXfrEntity userCard,
        CancellationToken cancellationToken)
    {
        UserCardAsyncInvokeCount++;
        return await Task.FromResult(UserCardAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByIdsAsync(
        IUserCardsByIdsXfrEntity userCards,
        CancellationToken cancellationToken)
    {
        UserCardsByIdsAsyncInvokeCount++;
        return await Task.FromResult(UserCardsByIdsAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByArtistAsync(
        IUserCardsArtistXfrEntity userCardsArtist,
        CancellationToken cancellationToken)
    {
        UserCardsByArtistAsyncInvokeCount++;
        return await Task.FromResult(UserCardsByArtistAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByNameAsync(
        IUserCardsNameXfrEntity userCardsName,
        CancellationToken cancellationToken)
    {
        UserCardsByNameAsyncInvokeCount++;
        return await Task.FromResult(UserCardsByNameAsyncResult).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsForSigningAsync(
        IUserCardsForSigningXfrEntity userCardsForSigning,
        CancellationToken cancellationToken)
    {
        UserCardsForSigningAsyncInvokeCount++;
        return await Task.FromResult(UserCardsForSigningAsyncResult).ConfigureAwait(false);
    }
}
