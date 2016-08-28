using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoBannedUrl<TDbContext> : SxDbRepository<int, SxBannedUrl, TDbContext> where TDbContext : SxDbContext
    {
        public override SxBannedUrl[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dbu.Id",
                "dbu.Url",
                "dbu.Couse",
                "dbu.DateCreate"
            }));
            sb.Append(" FROM D_BANNED_URL AS dbu ");

            object param = null;
            var gws = getBannedUrlWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dbu.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_BANNED_URL AS dbu ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBannedUrl>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getBannedUrlWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dbu.Url LIKE '%'+@url+'%' OR @url IS NULL) ");
            query.Append(" AND (dbu.Couse LIKE '%'+@couse+'%' OR @couse IS NULL) ");

            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;
            var couse = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Couse != null ? (string)filter.WhereExpressionObject.Couse : null;

            param = new
            {
                url = url,
                couse= couse
            };

            return query.ToString();
        }

        public string[] GetAllUrls()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<string>("dbo.get_banned_urls").ToArray();
                return data;
            }
        }

        public override void Delete(SxBannedUrl model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_banned_url @bannedUrlId", new { bannedUrlId=model.Id });
            }
        }
    }
}
