using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoMaterialTag<TDbContext> : SxDbRepository<string, SxMaterialTag, TDbContext> where TDbContext : SxDbContext
    {
        public override SxMaterialTag[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dmt.Id",
                "dmt.DateCreate",
                "dmt.MaterialId",
                "dmt.ModelCoreType"
            }));
            sb.Append(@" FROM D_MATERIAL_TAG AS dmt ");

            object param = null;
            var gws = getMaterialTagWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dmt.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_AFFILIATE_LINK AS dal ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxMaterialTag>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getMaterialTagWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dmt.Id LIKE '%'+@id+'%' OR @id IS NULL)");
            query.Append(" AND (dmt.MaterialId=@mid AND dmt.ModelCoreType=@mct) ");

            var id = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Id != null ? (string)filter.WhereExpressionObject.Id : null;

            param = new
            {
                id = id,
                mid = filter.MaterialId,
                mct = (byte)filter.ModelCoreType
            };

            return query.ToString();
        }

        public SxVMMaterialTag[] GetCloud(SxFilter filter, int amount = 50)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMMaterialTag>("dbo.get_material_cloud @amount, @mid, @mct", new
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
