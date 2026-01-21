using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal interface IUserSetCardArgEntityValidator : IValidatorAction<IUserSetCardArgEntity, IOperationResponse<IUserSetCardOufEntity>>;
