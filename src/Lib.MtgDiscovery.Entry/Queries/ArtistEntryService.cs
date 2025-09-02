using System.Threading.Tasks;
using Lib.MtgDiscovery.Data.Apis;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Queries.Mappers;
using Lib.MtgDiscovery.Entry.Queries.Validators;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Queries;

internal sealed class ArtistEntryService : IArtistEntryService
{
    private readonly IArtistDataService _artistDataService;
    private readonly IArtistSearchTermArgEntityValidator _searchTermValidator;
    private readonly IArtistIdArgEntityValidator _artistIdValidator;
    private readonly IArtistSearchTermArgsToItrMapper _searchTermMapper;
    private readonly IArtistIdArgsToItrMapper _artistIdMapper;

    public ArtistEntryService(ILogger logger) : this(
        new DataService(logger),
        new ArtistSearchTermArgEntityValidatorContainer(),
        new ArtistIdArgEntityValidatorContainer(),
        new ArtistSearchTermArgsToItrMapper(),
        new ArtistIdArgsToItrMapper())
    {
    }

    private ArtistEntryService(
        IArtistDataService artistDataService,
        IArtistSearchTermArgEntityValidator searchTermValidator,
        IArtistIdArgEntityValidator artistIdValidator,
        IArtistSearchTermArgsToItrMapper searchTermMapper,
        IArtistIdArgsToItrMapper artistIdMapper)
    {
        _artistDataService = artistDataService;
        _searchTermValidator = searchTermValidator;
        _artistIdValidator = artistIdValidator;
        _searchTermMapper = searchTermMapper;
        _artistIdMapper = artistIdMapper;
    }

    public async Task<IOperationResponse<IArtistSearchResultCollectionItrEntity>> ArtistSearchAsync(IArtistSearchTermArgEntity searchTerm)
    {
        IValidatorActionResult<IOperationResponse<IArtistSearchResultCollectionItrEntity>> result = await _searchTermValidator.Validate(searchTerm).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IArtistSearchTermItrEntity mappedArgs = await _searchTermMapper.Map(searchTerm).ConfigureAwait(false);
        return await _artistDataService.ArtistSearchAsync(mappedArgs).ConfigureAwait(false);
    }

    public async Task<IOperationResponse<ICardItemCollectionItrEntity>> CardsByArtistAsync(IArtistIdArgEntity artistId)
    {
        IValidatorActionResult<IOperationResponse<ICardItemCollectionItrEntity>> result = await _artistIdValidator.Validate(artistId).ConfigureAwait(false);

        if (result.IsNotValid()) return result.FailureStatus();

        IArtistIdItrEntity mappedArgs = await _artistIdMapper.Map(artistId).ConfigureAwait(false);
        return await _artistDataService.CardsByArtistAsync(mappedArgs).ConfigureAwait(false);
    }
}