using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators.Delete;

internal sealed class IsNotNullDeleteCollectionArgEntityValidator : OperationResponseValidator<IDeleteCollectionArgEntity, IDeleteCollectionItrEntity>
{
    public IsNotNullDeleteCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IDeleteCollectionArgEntity>
    {
        public Task<bool> IsValid(IDeleteCollectionArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Delete collection argument cannot be null";
    }
}
