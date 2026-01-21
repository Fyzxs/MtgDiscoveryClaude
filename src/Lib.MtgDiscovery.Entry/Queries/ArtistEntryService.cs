using System.Threading.Tasks;
using Lib.Domain.Artists.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators.Artists;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class ArtistEntryService : IArtistEntryService
{
    private readonly IArtistDomainService _artistDomainService;
    private readonly IArtistSearchTermArgEntityValidator _searchTermValidator;
    private readonly IArtistIdArgEntityValidator _artistIdValidator;
    private readonly IArtistNameArgEntityValidator _artistNameValidator;
    private readonly IArtistSearchTermArgsToItrMapper _searchTermMapper;
    private readonly IArtistIdArgsToItrMapper _artistIdMapper;
    private readonly IArtistNameArgsToItrMapper _artistNameMapper;

    public ArtistEntryService(ILogger logger) : this(
        new ArtistDomainService(logger),
        new ArtistSearchTermArgEntityValidatorContainer(),
        new ArtistIdArgEntityValidatorContainer(),
        new ArtistNameArgEntityValidatorContainer(),
        new ArtistSearchTermArgsToItrMapper(),
        new ArtistIdArgsToItrMapper(),
        new ArtistNameArgsToItrMapper())
    { }

    private ArtistEntryService(
        IArtistDomainService artistDataService,
        IArtistSearchTermArgEntityValidator searchTermValidator,
        IArtistIdArgEntityValidator artistIdValidator,
        IArtistNameArgEntityValidator artistNameValidator,
        IArtistSearchTermArgsToItrMapper searchTermMapper,
        IArtistIdArgsToItrMapper artistIdMapper,
        IArtistNameArgsToItrMapper artistNameMapper)
    {
        _artistDomainService = artistDataService;
        _searchTermValidator = searchTermValidator;
        _artistIdValidator = artistIdValidator;
        _artistNameValidator = artistNameValidator;
        _searchTermMapper = searchTermMapper;
        _artistIdMapper = artistIdMapper;
        _artistNameMapper = artistNameMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<IArtistSearchResultCollectionItrEntity>> result = await _searchTermValidator.Validate(searchTerm).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IArtistSearchTermItrEntity mappedArgs = await _searchTermMapper.Map(searchTerm).ConfigureAwait(false);
        return await _artistDomainService.ArtistSearchAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdArgEntity artistId)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> result = await _artistIdValidator.Validate(artistId).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IArtistIdItrEntity mappedArgs = await _artistIdMapper.Map(artistId).ConfigureAwait(false);
        return await _artistDomainService.CardsByArtistAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistNameAsync(IArtistNameArgEntity artistName)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> result = await _artistNameValidator.Validate(artistName).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IArtistNameItrEntity mappedArgs = await _artistNameMapper.Map(artistName).ConfigureAwait(false);
        return await _artistDomainService.CardsByArtistNameAsync(mappedArgs).ConfigureAwait(false);
    }
}
