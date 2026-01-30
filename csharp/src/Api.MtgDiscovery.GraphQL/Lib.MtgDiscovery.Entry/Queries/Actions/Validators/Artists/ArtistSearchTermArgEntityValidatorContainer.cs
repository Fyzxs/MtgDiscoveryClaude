using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Artists;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Artists;

internal sealed class ArtistSearchTermArgEntityValidatorContainer : ValidatorActionContainer<IArtistSearchTermArgEntity, IOperationResponse<IArtistSearchResultCollectionOufEntity>>, IArtistSearchTermArgEntityValidator
{
    public ArtistSearchTermArgEntityValidatorContainer() : base([
            new IsNotNullArtistSearchTermArgEntityValidator(),
            new HasValidArtistSearchTermArgEntityValidator(),
            new HasMinimumLengthArtistSearchTermArgEntityValidator(),
        ])
    { }
}
