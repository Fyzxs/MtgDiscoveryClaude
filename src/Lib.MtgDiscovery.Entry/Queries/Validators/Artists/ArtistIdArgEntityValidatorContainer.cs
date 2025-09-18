using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

internal sealed class ArtistIdArgEntityValidatorContainer : ValidatorActionContainer<IArtistIdArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>, IArtistIdArgEntityValidator
{
    public ArtistIdArgEntityValidatorContainer() : base([
            new IsNotNullArtistIdArgEntityValidator(),
            new HasValidArtistIdArgEntityValidator(),
        ])
    { }
}
internal sealed class IsNotNullArtistIdArgEntityValidator : OperationResponseValidator<IArtistIdArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullArtistIdArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistIdArgEntity>
    {
        public Task<bool> IsValid(IArtistIdArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist ID argument cannot be null";
    }
}

internal sealed class HasValidArtistIdArgEntityValidator : OperationResponseValidator<IArtistIdArgEntity, ICardItemCollectionOufEntity>
{
    public HasValidArtistIdArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistIdArgEntity>
    {
        public Task<bool> IsValid(IArtistIdArgEntity arg) => Task.FromResult(arg.ArtistId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist ID cannot be empty";
    }
}
