using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators;

internal sealed class HasValidNameCreateCollectionArgEntityValidator : OperationResponseValidator<ICreateCollectionArgEntity, ICollectionItrEntity>
{
    public HasValidNameCreateCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICreateCollectionArgEntity>
    {
        public Task<bool> IsValid(ICreateCollectionArgEntity arg) => Task.FromResult(arg.Name.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection name cannot be empty";
    }
}
