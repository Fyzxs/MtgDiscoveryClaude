using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.SealedProducts;

internal sealed class HasValidSetCodeSealedProductsBySetCodeArgEntityValidator : OperationResponseValidator<ISealedProductsBySetCodeArgEntity, IEnumerable<ISealedProductOufEntity>>
{
    public HasValidSetCodeSealedProductsBySetCodeArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<ISealedProductsBySetCodeArgEntity>
    {
        public Task<bool> IsValid(ISealedProductsBySetCodeArgEntity arg) => Task.FromResult(arg.SetCode.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "SetCode cannot be empty";
    }
}
