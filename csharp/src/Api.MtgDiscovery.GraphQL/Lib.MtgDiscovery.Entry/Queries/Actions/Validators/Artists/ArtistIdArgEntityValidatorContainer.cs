using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Artists;

internal sealed class ArtistIdArgEntityValidatorContainer : ValidatorActionContainer<IArtistIdArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>, IArtistIdArgEntityValidator
{
    public ArtistIdArgEntityValidatorContainer() : base([
            new IsNotNullArtistIdArgEntityValidator(),
            new HasValidArtistIdArgEntityValidator(),
        ])
    { }
}
