using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Collections;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Collections.Validators;

internal sealed class HasValidVisibilityCreateCollectionArgEntityValidator : OperationResponseValidator<ICreateCollectionArgEntity, ICollectionItrEntity>
{
    public HasValidVisibilityCreateCollectionArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ICreateCollectionArgEntity>
    {
        public Task<bool> IsValid(ICreateCollectionArgEntity arg) => Task.FromResult(string.IsNullOrWhiteSpace(arg.Visibility) || arg.Visibility is "private" or "public");
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Collection visibility must be 'private' or 'public'";
    }
}
