using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators;

internal sealed class HasValidTypeCreateCollectionArgEntityValidator : OperationResponseValidator<ICreateCollectionArgEntity, ICollectionItrEntity>
{
    public HasValidTypeCreateCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICreateCollectionArgEntity>
    {
        public Task<bool> IsValid(ICreateCollectionArgEntity arg) =>
            Task.FromResult(arg.Type is "custom" or "cube" or "trade");
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection type must be 'custom', 'cube', or 'trade'";
    }
}
