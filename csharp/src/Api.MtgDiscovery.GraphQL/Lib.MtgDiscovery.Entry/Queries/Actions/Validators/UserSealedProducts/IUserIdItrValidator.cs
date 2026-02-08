using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs.UserSealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.UserSealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserSealedProducts;

internal interface IUserIdItrValidator : IValidatorAction<IUserIdItrEntity, IOperationResponse<IEnumerable<IUserSealedProductOufEntity>>>;
