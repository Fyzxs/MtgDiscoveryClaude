using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Visibility;

internal sealed class IsNotNullUpdateCollectionVisibilityArgEntityValidator : OperationResponseValidator<IUpdateCollectionVisibilityArgEntity, IUpdateCollectionVisibilityItrEntity>
{
    public IsNotNullUpdateCollectionVisibilityArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUpdateCollectionVisibilityArgEntity>
    {
        public Task<bool> IsValid(IUpdateCollectionVisibilityArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Update collection visibility argument cannot be null";
    }
}
