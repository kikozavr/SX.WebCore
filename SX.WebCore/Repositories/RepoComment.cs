using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;

namespace SX.WebCore.Repositories
{
    public sealed class RepoComment<TDbContext> : SxDbRepository<int, SxComment, TDbContext> where TDbContext: SxDbContext
    {
        public override IQueryable<SxComment> Query(SxFilter filter)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxComment, SxAppUser, SxComment>("get_material_comments @mid, @mct", (c, u)=> {
                    c.User = u ?? new SxAppUser { NikName=c.UserName};
                    return c;
                }, new { mid = filter.MaterialId, mct = filter.ModelCoreType });
                return data.AsQueryable();
            }
        }
    }
}
