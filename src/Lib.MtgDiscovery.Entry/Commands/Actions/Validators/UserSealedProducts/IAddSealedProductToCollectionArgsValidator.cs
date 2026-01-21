using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.SealedProducts;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Validators.UserSealedProducts;

internal interface IAddSealedProductToCollectionArgsValidator : IValidatorAction<IAddSealedProductToCollectionArgsEntity, IOperationResponse<List<SealedProductOutEntity>>>;
