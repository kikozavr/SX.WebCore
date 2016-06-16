using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBannedUrl<TDbContext> : SxDbRepository<int, SxBannedUrl, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxBannedUrl> Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dbu.Id", "dbu.Url", "dbu.Couse" });
            query += " FROM D_BANNED_URL AS dbu ";

            object param = null;
            query += getBannedUrlWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("dbu.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBannedUrl>(query, param: param);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_BANNED_URL AS dbu ";

            object param = null;
            query += getBannedUrlWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        public string[] GetAllUrls()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<string>("get_banned_urls").ToArray();
                return data;
            }
        }

        private static string getBannedUrlWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dbu.Url LIKE '%'+@url+'%' OR @url IS NULL) ";
            query += " AND (dbu.Couse LIKE '%'+@couse+'%' OR @couse IS NULL) ";

            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;
            var couse = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Couse != null ? (string)filter.WhereExpressionObject.Couse : null;

            param = new
            {
                url = url,
                couse= couse
            };

            return query;
        }
    }
}
