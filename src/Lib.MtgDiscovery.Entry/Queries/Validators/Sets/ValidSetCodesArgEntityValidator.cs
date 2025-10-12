using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Sets;

internal sealed class ValidSetCodesArgEntityValidator : OperationResponseValidator<ISetCodesArgEntity, ISetItemCollectionOufEntity>
{
    public ValidSetCodesArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISetCodesArgEntity>
    {
        public Task<bool> IsValid(ISetCodesArgEntity arg) => Task.FromResult(arg.SetCodes.All(code => StringExtensions.IzNotNullOrWhiteSpace(code)));
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Provided list has invalid entries";
    }
}