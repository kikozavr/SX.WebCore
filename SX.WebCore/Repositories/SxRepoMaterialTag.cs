using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoMaterialTag<TDbContext> : SxDbRepository<string, SxMaterialTag, TDbContext> where TDbContext : SxDbContext
    {
        public override SxMaterialTag[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dmt.Id", "dmt.DateCreate", "dmt.MaterialId", "dmt.ModelCoreType" });
            query += " FROM D_MATERIAL_TAG AS dmt";

            object param = null;
            query += getMaterialTagWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dmt.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);
            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxMaterialTag>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_MATERIAL_TAG AS dmt";

            object param = null;
            query += getMaterialTagWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getMaterialTagWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dmt.Id LIKE '%'+@id+'%' OR @id IS NULL)";
            query += " AND (dmt.MaterialId=@mid AND dmt.ModelCoreType=@mct) ";

            var id = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Id != null ? (string)filter.WhereExpressionObject.Id : null;

            param = new
            {
                id = id,
                mid = filter.MaterialId,
                mct = (byte)filter.ModelCoreType
            };

            return query;
        }

        public SxVMMaterialTag[] GetCloud(SxFilter filter, int amount = 50)
        {
            var query = @"SELECT TOP(@amount) x.Title,
       SUM(x.[Count])  AS [Count],
       (CASE WHEN SUM(x.IsCurrent) >= 1 THEN 1 ELSE 0 END) AS IsCurrent
FROM   (
           SELECT dmt.Id          AS Title,
                  COUNT(1)        AS [Count],
                  (
                      CASE 
                           WHEN (dmt.MaterialId = @mid OR @mid IS NULL)
                      AND (dmt.ModelCoreType = @mct OR @mct IS NULL) THEN 1 ELSE 
                          0 END
                  )               AS IsCurrent
           FROM   D_MATERIAL_TAG  AS dmt  WHERE dmt.ModelCoreType=@mct
           GROUP BY
                  dmt.Id,
                  dmt.MaterialId,
                  dmt.ModelCoreType
       )                  x
GROUP BY
       x.Title
ORDER BY
       x.Title";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMMaterialTag>(query, new
                {
                    mid = filter.MaterialId,
                    mct = filter.ModelCoreType,
                    amount = amount
                });
                return data.ToArray();
            }
        }
    }
}
