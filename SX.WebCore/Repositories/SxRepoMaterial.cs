using Dapper;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.Enums;

namespace SX.WebCore.Repositories
{
    public abstract class SxRepoMaterial<TModel, TDbContext> : SxDbRepository<int, TModel, TDbContext>
        where TModel : SxDbModel<int>
        where TDbContext : SxDbContext
    {
        public SxDateStatistic[] DateStatistic(ModelCoreType mct)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxDateStatistic>("dbo.get_material_date_statistic @mct", new { mct = mct });
                return data.ToArray();
            }
        }
    }
}
