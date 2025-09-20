using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Domain.Artists.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Artists;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.DataModels.Entities.Outs.Artists;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries.Artists;

internal sealed class ArtistSearchEntryService : IArtistSearchEntryService
{
    private readonly IArtistDomainService _artistDomainService;
    private readonly IArtistSearchTermArgEntityValidator _artistSearchTermArgEntityValidator;
    private readonly IArtistSearchTermArgToItrMapper _artistSearchTermArgToItrMapper;
    private readonly IArtistSearchResultCollectionOufToOutMapper _artistSearchResultOufToOutMapper;

    public ArtistSearchEntryService(ILogger logger) : this(
        new ArtistDomainService(logger),
        new ArtistSearchTermArgEntityValidatorContainer(),
        new ArtistSearchTermArgToItrMapper(),
        new ArtistSearchResultCollectionOufToOutMapper())
    { }

    private ArtistSearchEntryService(
        IArtistDomainService artistDomainService,
        IArtistSearchTermArgEntityValidator artistSearchTermArgEntityValidator,
        IArtistSearchTermArgToItrMapper artistSearchTermArgToItrMapper,
        IArtistSearchResultCollectionOufToOutMapper artistSearchResultOufToOutMapper)
    {
        _artistDomainService = artistDomainService;
        _artistSearchTermArgEntityValidator = artistSearchTermArgEntityValidator;
        _artistSearchTermArgToItrMapper = artistSearchTermArgToItrMapper;
        _artistSearchResultOufToOutMapper = artistSearchResultOufToOutMapper;
    }

    public async Task<IOperationResponse<List<ArtistSearchResultOutEntity>>> Execute(IArtistSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<IArtistSearchResultCollectionOufEntity>> validatorResult = await _artistSearchTermArgEntityValidator.Validate(searchTerm).ConfigureAwait(false);
        if (validatorResult.IsNotValid()) return new FailureOperationResponse<List<ArtistSearchResultOutEntity>>(validatorResult.FailureStatus().OuterException);

        IArtistSearchTermItrEntity itrEntity = await _artistSearchTermArgToItrMapper.Map(searchTerm).ConfigureAwait(false);
        IOperationResponse<IArtistSearchResultCollectionOufEntity> opResponse = await _artistDomainService.ArtistSearchAsync(itrEntity).ConfigureAwait(false);
        if (opResponse.IsFailure) return new FailureOperationResponse<List<ArtistSearchResultOutEntity>>(opResponse.OuterException);

        List<ArtistSearchResultOutEntity> outEntities = await _artistSearchResultOufToOutMapper.Map(opResponse.ResponseData).ConfigureAwait(false);
        return new SuccessOperationResponse<List<ArtistSearchResultOutEntity>>(outEntities);
    }
}