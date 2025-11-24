using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Oufs.UserSetCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserSetCards;

internal interface IAllUserSetCardsArgEntityValidator : IValidatorAction<IAllUserSetCardsArgEntity, IOperationResponse<IEnumerable<IUserSetCardOufEntity>>>;
