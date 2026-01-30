using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.SealedProducts;

internal sealed class IsNotNullSealedProductsBySetCodeArgEntityValidator : OperationResponseValidator<ISealedProductsBySetCodeArgEntity, IEnumerable<ISealedProductOufEntity>>
{
    public IsNotNullSealedProductsBySetCodeArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISealedProductsBySetCodeArgEntity>
    {
        public Task<bool> IsValid(ISealedProductsBySetCodeArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "Sealed products by set code argument cannot be null";
    }
}
