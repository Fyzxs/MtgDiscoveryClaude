using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

internal sealed class ArtistNameArgEntityValidatorContainer : ValidatorActionContainer<IArtistNameArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>, IArtistNameArgEntityValidator
{
    public ArtistNameArgEntityValidatorContainer() : base([
            new IsNotNullArtistNameArgEntityValidator(),
            new HasValidArtistNameArgEntityValidator(),
            new HasMinimumLengthArtistNameArgEntityValidator(),
        ])
    { }
}

internal sealed class IsNotNullArtistNameArgEntityValidator : OperationResponseValidator<IArtistNameArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullArtistNameArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistNameArgEntity>
    {
        public Task<bool> IsValid(IArtistNameArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist name argument cannot be null";
    }
}

internal sealed class HasValidArtistNameArgEntityValidator : OperationResponseValidator<IArtistNameArgEntity, ICardItemCollectionOufEntity>
{
    public HasValidArtistNameArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistNameArgEntity>
    {
        public Task<bool> IsValid(IArtistNameArgEntity arg) => Task.FromResult(arg.ArtistName.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist name cannot be empty";
    }
}

internal sealed class HasMinimumLengthArtistNameArgEntityValidator : OperationResponseValidator<IArtistNameArgEntity, ICardItemCollectionOufEntity>
{
    public HasMinimumLengthArtistNameArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IArtistNameArgEntity>
    {
        public Task<bool> IsValid(IArtistNameArgEntity arg)
        {
            if (arg?.ArtistName == null) return Task.FromResult(false);

            // Normalize the artist name to check minimum length
            string normalized = new([.. arg.ArtistName
                .ToLowerInvariant()
                .Where(char.IsLetter)]);

            return Task.FromResult(normalized.Length >= 3);
        }
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Artist name must contain at least 3 letters";
    }
}
