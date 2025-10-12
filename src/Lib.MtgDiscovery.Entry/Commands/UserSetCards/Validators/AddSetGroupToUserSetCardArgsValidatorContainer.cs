using Lib.Aggregator.UserSetCards.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal sealed class AddSetGroupToUserSetCardArgsValidatorContainer : ValidatorActionContainer<IAddSetGroupToUserSetCardArgsEntity, IOperationResponse<IUserSetCardOufEntity>>, IAddSetGroupToUserSetCardArgsValidator
{
    public AddSetGroupToUserSetCardArgsValidatorContainer() : base([
            new AuthUserNotEmptyValidator(),
            new SetIdNotEmptyValidator(),
            new SetGroupIdNotEmptyValidator(),
            new CountNotNegativeValidator(),
        ])
    { }
}
