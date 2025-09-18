using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using App.MtgDiscovery.GraphQL.Entities.Outs.Cards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class ArtistQueryMethods
{
    private readonly ICardItemItrToOutMapper _scryfallCardMapper;
    private readonly ICardItemCollectionItrToOutMapper _cardCollectionMapper;
    private readonly IArtistSearchResultCollectionItrToOutMapper _artistSearchMapper;
    private readonly IEntryService _entryService;

    public ArtistQueryMethods(ILogger logger) : this(
        new CardItemItrToOutMapper(),
        new CardItemCollectionItrToOutMapper(),
        new ArtistSearchResultCollectionItrToOutMapper(),
        new EntryService(logger))
    {
    }

    private ArtistQueryMethods(
        ICardItemItrToOutMapper scryfallCardMapper,
        ICardItemCollectionItrToOutMapper cardCollectionMapper,
        IArtistSearchResultCollectionItrToOutMapper artistSearchMapper,
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
        IOperationResponse<IArtistSearchResultCollectionItrEntity> response = await _entryService.ArtistSearchAsync(searchTerm).ConfigureAwait(false);

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
        IOperationResponse<ICardItemCollectionItrEntity> response = await _entryService.CardsByArtistAsync(artistId).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = results };
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtistName(ArtistNameArgEntity artistName)
    {
        IOperationResponse<ICardItemCollectionItrEntity> response = await _entryService.CardsByArtistNameAsync(artistName).ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<CardItemOutEntity> results = await _cardCollectionMapper.Map(response.ResponseData.Data).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = results };
    }
}
