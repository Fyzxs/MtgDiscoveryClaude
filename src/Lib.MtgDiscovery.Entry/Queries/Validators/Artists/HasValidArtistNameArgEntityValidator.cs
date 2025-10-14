using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

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
