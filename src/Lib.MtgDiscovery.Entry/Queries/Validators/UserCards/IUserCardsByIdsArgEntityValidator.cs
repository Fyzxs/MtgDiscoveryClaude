using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.UserCards;

internal interface IUserCardsByIdsArgEntityValidator : IValidatorAction<IUserCardsByIdsArgEntity, IOperationResponse<IEnumerable<IUserCardOufEntity>>>;
