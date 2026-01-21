using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal interface IUserCardsByIdsArgEntityValidator : IValidatorAction<IUserCardsByIdsArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>;
