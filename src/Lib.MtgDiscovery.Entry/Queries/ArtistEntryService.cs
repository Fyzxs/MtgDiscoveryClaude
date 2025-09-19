using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Artists.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Artists;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class ArtistEntryService : IArtistEntryService
{
    private readonly IArtistDomainService _artistDomainService;
    private readonly IArtistSearchTermArgEntityValidator _artistSearchTermArgEntityValidator;
    private readonly IArtistIdArgEntityValidator _artistIdArgEntityValidator;
    private readonly IArtistNameArgEntityValidator _artistNameArgEntityValidator;
    private readonly IArtistSearchTermArgsToItrMapper _artistSearchTermArgsToItrMapper;
    private readonly IArtistIdArgsToItrMapper _artistIdArgsToItrMapper;
    private readonly IArtistNameArgsToItrMapper _artistNameArgsToItrMapper;
    private readonly IArtistSearchResultCollectionOufToOutMapper _artistSearchResultOufToOutMapper;
    private readonly ICollectionCardItemOufToOutMapper _cardItemOufToOutMapper;

    public ArtistEntryService(ILogger logger) : this(
        new ArtistDomainService(logger),
        new ArtistSearchTermArgEntityValidatorContainer(),
        new ArtistIdArgEntityValidatorContainer(),
        new ArtistNameArgEntityValidatorContainer(),
        new ArtistSearchTermArgsToItrMapper(),
        new ArtistIdArgsToItrMapper(),
        new ArtistNameArgsToItrMapper(),
        new ArtistSearchResultCollectionOufToOutMapper(),
        new CollectionCardItemOufToOutMapper())
    { }

    private ArtistEntryService(
        IArtistDomainService artistDomainService,
        IArtistSearchTermArgEntityValidator artistSearchTermArgEntityValidator,
        IArtistIdArgEntityValidator artistIdArgEntityValidator,
        IArtistNameArgEntityValidator artistNameArgEntityValidator,
        IArtistSearchTermArgsToItrMapper artistSearchTermArgsToItrMapper,
        IArtistIdArgsToItrMapper artistIdArgsToItrMapper,
        IArtistNameArgsToItrMapper artistNameArgsToItrMapper,
        IArtistSearchResultCollectionOufToOutMapper artistSearchResultOufToOutMapper,
        ICollectionCardItemOufToOutMapper cardItemOufToOutMapper)
    {
        _artistDomainService = artistDomainService;
        _artistSearchTermArgEntityValidator = artistSearchTermArgEntityValidator;
        _artistIdArgEntityValidator = artistIdArgEntityValidator;
        _artistNameArgEntityValidator = artistNameArgEntityValidator;
        _artistSearchTermArgsToItrMapper = artistSearchTermArgsToItrMapper;
        _artistIdArgsToItrMapper = artistIdArgsToItrMapper;
        _artistNameArgsToItrMapper = artistNameArgsToItrMapper;
        _artistSearchResultOufToOutMapper = artistSearchResultOufToOutMapper;
        _cardItemOufToOutMapper = cardItemOufToOutMapper;
    }

    public async Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<IArtistSearchResultCollectionOufEntity>> validatorResult = await _artistSearchTermArgEntityValidator.Validate(searchTerm).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<ArtistSearchResultOutEntity>>(validatorResult.FailureStatus().OuterException);

        IArtistSearchTermItrEntity itrEntity = await _artistSearchTermArgsToItrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<IArtistSearchResultCollectionOufEntity> opResponse = await _artistDomainService.ArtistSearchAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<ArtistSearchResultOutEntity>>(opResponse.OuterException);

        List<ArtistSearchResultOutEntity> outEntities = await _artistSearchResultOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ArtistSearchResultOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistAsync(IArtistIdArgEntity artistId)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _artistIdArgEntityValidator.Validate(artistId).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        IArtistIdItrEntity itrEntity = await _artistIdArgsToItrMapper.Map(artistId).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _artistDomainService.CardsByArtistAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }

    public async Task<IOperationResponse<List<CardItemOutEntity>>> CardsByArtistNameAsync(IArtistNameArgEntity artistName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionOufEntity>> validatorResult = await _artistNameArgEntityValidator.Validate(artistName).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<CardItemOutEntity>>(validatorResult.FailureStatus().OuterException);

        IArtistNameItrEntity itrEntity = await _artistNameArgsToItrMapper.Map(artistName).ConfigureAwait(false);
        IOperationResponse<ICardItemCollectionOufEntity> opResponse = await _artistDomainService.CardsByArtistNameAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<CardItemOutEntity>>(opResponse.OuterException);

        List<CardItemOutEntity> outEntities = await _cardItemOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<CardItemOutEntity>>(outEntities);
    }
}
