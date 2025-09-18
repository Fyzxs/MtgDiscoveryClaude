using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class ArtistQueryMethods
{
    private readonly ICardItemOufToOutMapper _scryfallCardMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardCollectionMapper;
    private readonly IArtistSearchResultCollectionOufToOutMapper _artistSearchMapper;
    private readonly IEntryService _entryService;

    public ArtistQueryMethods(ILogger logger) : this(
        new CardItemOufToOutMapper(),
        new CollectionCardItemOufToOutMapper(),
        new ArtistSearchResultCollectionOufToOutMapper(),
        new EntryService(logger))
    {
    }

    private ArtistQueryMethods(
        ICardItemOufToOutMapper scryfallCardMapper,
        ICollectionCardItemOufToOutMapper cardCollectionMapper,
        IArtistSearchResultCollectionOufToOutMapper artistSearchMapper,
        IEntryService entryService)
    {
        _scryfallCardMapper = scryfallCardMapper;
        _cardCollectionMapper = cardCollectionMapper;
        _artistSearchMapper = artistSearchMapper;
        _entryService = entryService;
    }

    [GraphQLType(typeof(ArtistSearchResponseModelUnionType))]
    public async Task<ResponseModel> ArtistSearch(ArtistSearchTermArgEntity searchTerm)
    {
        IOperationResponse<IArtistSearchResultCollectionOufEntity> response = await _entryService.ArtistSearchAsync(searchTerm).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<ArtistSearchResultOutEntity> results = await _artistSearchMapper.Map(response.ResponseData.Artists).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<ArtistSearchResultOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtist(ArtistIdArgEntity artistId)
    {
        IOperationResponse<ICardItemCollectionOufEntity> response = await _entryService.CardsByArtistAsync(artistId).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        ICollection<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = results.ToList() };
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtistName(ArtistNameArgEntity artistName)
    {
        IOperationResponse<ICardItemCollectionOufEntity> response = await _entryService.CardsByArtistNameAsync(artistName).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        ICollection<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = results.ToList() };
    }
}
