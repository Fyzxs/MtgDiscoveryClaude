using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal sealed class IsNotNullSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public IsNotNullSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided object is null";
    }
}
