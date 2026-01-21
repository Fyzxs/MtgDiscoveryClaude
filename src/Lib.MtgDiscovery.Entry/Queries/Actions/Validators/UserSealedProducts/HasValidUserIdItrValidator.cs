using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserSealedProducts;

internal sealed class HasValidUserIdItrValidator : OperationResponseValidator<IUserIdItrEntity, IEnumerable<IUserSealedProductItrEntity>>
{
    public HasValidUserIdItrValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IUserIdItrEntity>
    {
        public Task<bool> IsValid(IUserIdItrEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User ID cannot be empty";
    }
}
