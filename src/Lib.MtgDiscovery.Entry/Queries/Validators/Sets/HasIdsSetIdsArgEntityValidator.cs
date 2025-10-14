using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal sealed class HasIdsSetIdsArgEntityValidator : OperationResponseValidator<ISetIdsArgEntity, ISetItemCollectionOufEntity>
{
    public HasIdsSetIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetIdsArgEntity>
    {
        public Task<bool> IsValid(ISetIdsArgEntity arg) => Task.FromResult(0 < arg.SetIds.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is empty";
    }
}
