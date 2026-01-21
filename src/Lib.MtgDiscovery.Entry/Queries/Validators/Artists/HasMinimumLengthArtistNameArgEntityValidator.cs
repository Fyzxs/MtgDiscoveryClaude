using System.Linq;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

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