using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class ArtistSearchTermArgEntityValidatorContainer : ValidatorActionContainer<IArtistSearchTermArgEntity, IOperationResponse<IArtistSearchResultCollectionItrEntity>>, IArtistSearchTermArgEntityValidator
{
    public ArtistSearchTermArgEntityValidatorContainer() : base([
            new IsNotNullArtistSearchTermArgEntityValidator(),
            new HasValidArtistSearchTermArgEntityValidator(),
            new HasMinimumLengthArtistSearchTermArgEntityValidator(),
        ])
    { }
}