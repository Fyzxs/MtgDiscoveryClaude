using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal sealed class ValidSetIdsArgEntityValidator : OperationResponseValidator<ISetIdsArgEntity, ISetItemCollectionOufEntity>
{
    public ValidSetIdsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetIdsArgEntity>
    {
        public Task<bool> IsValid(ISetIdsArgEntity arg) => Task.FromResult(arg.SetIds.All(id => StringExtensions.IzNotNullOrWhiteSpace(id)));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list has invalid entries";
    }
}