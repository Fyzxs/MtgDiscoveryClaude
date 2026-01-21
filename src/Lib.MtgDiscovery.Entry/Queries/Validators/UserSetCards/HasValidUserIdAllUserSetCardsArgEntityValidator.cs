using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Lib.Universal.Extensions;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal sealed class HasValidUserIdAllUserSetCardsArgEntityValidator : OperationResponseValidator<IAllUserSetCardsArgEntity, IEnumerable<IUserSetCardOufEntity>>
{
    public HasValidUserIdAllUserSetCardsArgEntityValidator() : base(new Validator(), new Message())
    { }

    public sealed class Validator : IValidator<IAllUserSetCardsArgEntity>
    {
        public Task<bool> IsValid(IAllUserSetCardsArgEntity arg) => Task.FromResult(arg.UserId.IzNotNullOrWhiteSpace());
    }

    public sealed class Message : OperationResponseMessage
    {
        public override string AsSystemType() => "User Id cannot be empty";
    }
}
