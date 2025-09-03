using Lib.MtgDiscovery.Entry.Queries.Validators.Components;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal sealed class ArtistNameArgEntityValidatorContainer : ValidatorActionContainer<IArtistNameArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>, IArtistNameArgEntityValidator
{
    public ArtistNameArgEntityValidatorContainer() : base([
            new IsNotNullArtistNameArgEntityValidator(),
            new HasValidArtistNameArgEntityValidator(),
            new HasMinimumLengthArtistNameArgEntityValidator(),
        ])
    { }
}