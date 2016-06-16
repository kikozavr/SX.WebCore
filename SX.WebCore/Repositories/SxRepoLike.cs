﻿using SX.WebCore.Abstract;
using System;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoLike<TDbContext> : SxDbRepository<Guid, SxLike, TDbContext> where TDbContext: SxDbContext
    {
    }
}
