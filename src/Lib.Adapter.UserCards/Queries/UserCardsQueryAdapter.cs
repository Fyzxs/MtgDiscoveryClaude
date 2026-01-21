using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Gophers;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Queries.Mappers;
using Lib.Cosmos.Apis.Ids;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Cosmos DB implementation of the user cards query adapter.
///
/// This class handles all Cosmos DB-specific user cards query operations,
/// implementing the specialized IUserCardsQueryAdapter interface.
/// The main UserCardsAdapterService delegates to this implementation.
/// </summary>
internal sealed class UserCardsQueryAdapter : IUserCardsQueryAdapter
{
    private readonly ICosmosInquisition<UserCardItemsBySetExtEntitys> _userCardsSetInquisition;
    private readonly ICosmosInquisition<UserCardItemsByArtistExtEntitys> _userCardsArtistInquisition;
    private readonly ICosmosInquisition<UserCardItemsByNameExtEntitys> _userCardsNameInquisition;
    private readonly ICosmosGopher _userCardsGopher;
    private readonly IUserCardsSetXfrToArgsMapper _setXfrToArgsMapper;
    private readonly IUserCardsArtistXfrToArgsMapper _artistXfrToArgsMapper;
    private readonly IUserCardsNameXfrToArgsMapper _nameXfrToArgsMapper;

    public UserCardsQueryAdapter(ILogger logger) : this(
        new UserCardItemsBySetInquisition(logger),
        new UserCardItemsByArtistInquisition(logger),
        new UserCardItemsByNameInquisition(logger),
        new UserCardsGopher(logger),
        new UserCardsSetXfrToArgsMapper(),
        new UserCardsArtistXfrToArgsMapper(),
        new UserCardsNameXfrToArgsMapper())
    { }

    private UserCardsQueryAdapter(
        ICosmosInquisition<UserCardItemsBySetExtEntitys> userCardsSetInquisition,
        ICosmosInquisition<UserCardItemsByArtistExtEntitys> userCardsArtistInquisition,
        ICosmosInquisition<UserCardItemsByNameExtEntitys> userCardsNameInquisition,
        ICosmosGopher userCardsGopher,
        IUserCardsSetXfrToArgsMapper setXfrToArgsMapper,
        IUserCardsArtistXfrToArgsMapper artistXfrToArgsMapper,
        IUserCardsNameXfrToArgsMapper nameXfrToArgsMapper)
    {
        _userCardsSetInquisition = userCardsSetInquisition;
        _userCardsArtistInquisition = userCardsArtistInquisition;
        _userCardsNameInquisition = userCardsNameInquisition;
        _userCardsGopher = userCardsGopher;
        _setXfrToArgsMapper = setXfrToArgsMapper;
        _artistXfrToArgsMapper = artistXfrToArgsMapper;
        _nameXfrToArgsMapper = nameXfrToArgsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsBySetAsync(IUserCardsSetXfrEntity userCardsSet)
    {
        UserCardItemsBySetExtEntitys args = await _setXfrToArgsMapper.Map(userCardsSet).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsSetInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={userCardsSet.UserId}] cards for [set]{userCardsSet.SetId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardAsync(IUserCardXfrEntity userCard)
    {
        ReadPointItem readPoint = new()
        {
            Id = new ProvidedCosmosItemId(userCard.CardId),
            Partition = new ProvidedPartitionKeyValue(userCard.UserId)
        };

        OpResponse<UserCardExtEntity> response = await _userCardsGopher.ReadAsync<UserCardExtEntity>(readPoint).ConfigureAwait(false);

        // Handle "not found" as successful with empty collection (HTTP 404 is valid for point reads)
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>([]);
        }

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve user card [user={userCard.UserId}] [card={userCard.CardId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>([response.Value]);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByIdsAsync(IUserCardsByIdsXfrEntity userCards)
    {
        const int BatchSize = 20;
        List<UserCardExtEntity> foundCards = [];

        // Process card IDs in batches
        foreach (IEnumerable<string> batch in userCards.CardIds.Chunk(BatchSize))
        {
            IEnumerable<Task<OpResponse<UserCardExtEntity>>> readTasks = batch.Select(cardId =>
                _userCardsGopher.ReadAsync<UserCardExtEntity>(new ReadPointItem
                {
                    Id = new ProvidedCosmosItemId(cardId),
                    Partition = new ProvidedPartitionKeyValue(userCards.UserId)
                }));

            OpResponse<UserCardExtEntity>[] responses = await Task.WhenAll(readTasks).ConfigureAwait(false);

            foundCards.AddRange(responses
                .Where(r => r.StatusCode != System.Net.HttpStatusCode.NotFound && r.IsSuccessful())
                .Select(r => r.Value));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(foundCards);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByArtistAsync(IUserCardsArtistXfrEntity userCardsArtist)
    {
        UserCardItemsByArtistExtEntitys args = await _artistXfrToArgsMapper.Map(userCardsArtist).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsArtistInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={userCardsArtist.UserId}] cards for [artist]{userCardsArtist.ArtistId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> UserCardsByNameAsync(IUserCardsNameXfrEntity userCardsName)
    {
        UserCardItemsByNameExtEntitys args = await _nameXfrToArgsMapper.Map(userCardsName).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsNameInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={userCardsName.UserId}] cards for [name]{userCardsName.CardName}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }
}
