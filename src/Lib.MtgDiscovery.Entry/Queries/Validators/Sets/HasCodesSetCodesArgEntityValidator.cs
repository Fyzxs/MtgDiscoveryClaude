using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal sealed class HasCodesSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public HasCodesSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(0 < arg.SetCodes.Count);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list is empty";
    }
}
