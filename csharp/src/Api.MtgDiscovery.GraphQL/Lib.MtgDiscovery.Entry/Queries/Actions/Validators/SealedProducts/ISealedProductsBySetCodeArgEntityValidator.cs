using System.Collections.Generic;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.SealedProducts;
using Lib.Shared.DataModels.Entities.Oufs.SealedProducts;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.SealedProducts;

internal interface ISealedProductsBySetCodeArgEntityValidator : IValidatorAction<ISealedProductsBySetCodeArgEntity, IOperationResponse<IEnumerable<ISealedProductOufEntity>>>;
