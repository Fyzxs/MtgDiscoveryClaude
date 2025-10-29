using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal sealed class IsNotNullAllUserSetCardsArgEntityValidator : OperationResponseValidator<IAllUserSetCardsArgEntity, IEnumerable<IUserSetCardOufEntity>>
{
    public IsNotNullAllUserSetCardsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAllUserSetCardsArgEntity>
    {
        public Task<bool> IsValid(IAllUserSetCardsArgEntity arg) => Task.FromResult(arg is not null);
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "AllUserSetCardsArgEntity cannot be null";
    }
}
