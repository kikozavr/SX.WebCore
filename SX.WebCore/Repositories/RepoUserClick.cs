using SX.WebCore.Abstract;
using System;

namespace SX.WebCore.Repositories
{
    public sealed class RepoUserClick<TDbContext> : SxDbRepository<Guid, SxUserClick, TDbContext> where TDbContext: SxDbContext
    {
    }
}
