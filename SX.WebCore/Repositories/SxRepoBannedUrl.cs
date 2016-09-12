using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoBannedUrl<TDbContext> : SxDbRepository<int, SxBannedUrl, TDbContext, SxVMBannedUrl> where TDbContext : SxDbContext
    {
        public override SxBannedUrl Create(SxBannedUrl model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxBannedUrl>("dbo.add_banned_url @url, @couse", new { url = model.Url, couse = model.Couse });
                return data.SingleOrDefault();
            }
        }

        public override SxVMBannedUrl[] Read(SxFilter filter)
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
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order, new Dictionary<string, string> {
                { "DateCreate", "dbu.DateCreate"}
            }));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_BANNED_URL AS dbu ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMBannedUrl>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        public override SxBannedUrl Update(SxBannedUrl model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxBannedUrl>("dbo.update_banned_url @id, @url, @couse", new { id=model.Id, url = model.Url, couse = model.Couse });
                return data.SingleOrDefault();
            }
        }

        public override void Delete(SxBannedUrl model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_banned_url @bannedUrlId", new { bannedUrlId=model.Id });
            }
        }

        public override SxBannedUrl GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxBannedUrl>("dbo.get_banned_url @bannedUrlId", new { bannedUrlId = id[0] });
                return data.SingleOrDefault();
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
    }
}
