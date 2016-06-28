using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoSeoKeyword<TDbContext> : SxDbRepository<int, SxSeoKeyword, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSeoKeyword[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] {
                    "dsk.Id", "dsk.Value"
                });
            query += @" FROM D_SEO_KEYWORD AS dsk ";

            object param = null;
            query += getSeoKeywordsWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dsk.Value", Direction = SortDirection.Asc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                //TODO: Change
                try
                {
                    var data = conn.Query<SxSeoKeyword>(query, param: param);
                    return data.ToArray();
                }
                catch
                {
                    return new SxSeoKeyword[0];
                }
            }
        }
        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SEO_KEYWORD AS dsk";

            object param = null;
            query += getSeoKeywordsWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSeoKeywordsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dsk.SeoTagsId=@stid)";
            query += " AND (dsk.Value LIKE '%'+@val+'%' OR @val IS NULL)";

            var val = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Value != null ? (string)filter.WhereExpressionObject.Value : null;
            var stid = filter.AddintionalInfo != null && filter.AddintionalInfo[0] != null ? filter.AddintionalInfo[0] : null;

            param = new
            {
                stid = stid,
                val = val
            };

            return query;
        }
    }
}
