using System;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators;

internal sealed class NameNotReservedCreateCollectionArgEntityValidator : OperationResponseValidator<ICreateCollectionArgEntity, ICollectionItrEntity>
{
    public NameNotReservedCreateCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICreateCollectionArgEntity>
    {
        public Task<bool> IsValid(ICreateCollectionArgEntity arg) =>
            Task.FromResult(string.Equals(arg.Name, "default", StringComparison.OrdinalIgnoreCase) is false);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection name 'default' is reserved";
    }
}
