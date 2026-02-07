using System.Collections.Generic;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.UserWishlistCards;

internal interface IGetUserWishlistEntryService : IOperationResponseService<IGetUserWishlistArgsEntity, List<CardItemOutEntity>>;
