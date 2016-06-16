using SX.WebCore.Abstract;
using System;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoVideo<TDbContext> : SxDbRepository<Guid, SxVideo, TDbContext> where TDbContext : SxDbContext
    {
        
    }
}
