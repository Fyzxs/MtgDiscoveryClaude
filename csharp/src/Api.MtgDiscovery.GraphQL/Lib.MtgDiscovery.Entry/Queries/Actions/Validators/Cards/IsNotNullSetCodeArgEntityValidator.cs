using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Cards;

internal sealed class IsNotNullSetCodeArgEntityValidator : OperationResponseValidator<ISetCodeArgEntity, ICardItemCollectionOufEntity>
{
    public IsNotNullSetCodeArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodeArgEntity>
    {
        public Task<bool> IsValid(ISetCodeArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Set code argument cannot be null";
    }
}
