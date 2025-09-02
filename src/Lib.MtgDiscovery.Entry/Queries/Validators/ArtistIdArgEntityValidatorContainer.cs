using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class ArtistIdArgEntityValidatorContainer : ValidatorActionContainer<IArtistIdArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, IArtistIdArgEntityValidator
{
    public ArtistIdArgEntityValidatorContainer() : base([
            new IsNotNullArtistIdArgEntityValidator(),
            new HasValidArtistIdArgEntityValidator(),
        ])
    { }
}