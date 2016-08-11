using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSeoKeyword<TDbContext> : SxDbRepository<int, SxSeoKeyword, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSeoKeyword[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                    "dsk.Id", "dsk.Value"
                }));
            sb.Append(@" FROM D_SEO_KEYWORD AS dsk ");

            object param = null;
            var gws = getSeoKeywordsWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dsk.Value", Direction = SortDirection.Asc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_SEO_KEYWORD AS dsk ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSeoKeyword>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getSeoKeywordsWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dsk.SeoTagsId=@stid)");
            query.Append(" AND (dsk.Value LIKE '%'+@val+'%' OR @val IS NULL)");

            var val = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Value != null ? (string)filter.WhereExpressionObject.Value : null;
            var stid = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? filter.AddintionalInfo[0] : null;

            param = new
            {
                stid = stid,
                val = val
            };

            return query.ToString();
        }
    }
}
