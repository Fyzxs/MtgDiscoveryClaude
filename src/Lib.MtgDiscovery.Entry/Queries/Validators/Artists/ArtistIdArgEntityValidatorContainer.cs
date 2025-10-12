using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

internal sealed class ArtistIdArgEntityValidatorContainer : ValidatorActionContainer<IArtistIdArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>, IArtistIdArgEntityValidator
{
    public ArtistIdArgEntityValidatorContainer() : base([
            new IsNotNullArtistIdArgEntityValidator(),
            new HasValidArtistIdArgEntityValidator(),
        ])
    { }
}
