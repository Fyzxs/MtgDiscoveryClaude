using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Validators;

internal interface IAddSetGroupToUserSetCardArgsValidator : IValidatorAction<IAddSetGroupToUserSetCardArgsEntity, IOperationResponse<IUserSetCardOufEntity>>;
