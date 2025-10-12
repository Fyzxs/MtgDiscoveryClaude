using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

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