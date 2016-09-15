using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoAnalizatorUrl : SxDbRepository<int, SxAnalizatorUrl, SxVMAnalizatorUrl>
    {

        public override SxVMAnalizatorUrl[] Read(SxFilter filter)
        {
            return new SxVMAnalizatorUrl[0];
        }
    }
}
