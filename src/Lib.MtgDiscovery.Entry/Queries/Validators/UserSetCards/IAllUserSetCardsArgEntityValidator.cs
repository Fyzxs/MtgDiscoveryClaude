using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserSetCards;

internal interface IAllUserSetCardsArgEntityValidator : IValidatorAction<IAllUserSetCardsArgEntity, IOperationResponse<IEnumerable<IUserSetCardOufEntity>>>;
