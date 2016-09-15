using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore.ViewModels;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoComment : SxDbRepository<int, SxComment, SxVMComment>
    {
        public override SxVMComment[] Read(SxFilter filter)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMComment, SxVMAppUser, SxVMComment>("dbo.get_material_comments @mid, @mct", (c, u) => {
                    c.User = u ?? new SxVMAppUser { NikName = c.UserName };
                    return c;
                }, new { mid = filter.MaterialId, mct = filter.ModelCoreType }, splitOn:"Id");

                return data.ToArray();
            }
        }
    }
}
