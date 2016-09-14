using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoAnalizatorUrl<TDbContext> : SxDbRepository<int, SxAnalizatorUrl, TDbContext, SxVMAnalizatorUrl>
        where TDbContext : SxDbContext
    {

        public override SxVMAnalizatorUrl[] Read(SxFilter filter)
        {
            return new SxVMAnalizatorUrl[0];
        }
    }
}
