using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoBannerGroup<TDbContext> : SxDbRepository<Guid, SxBannerGroup, TDbContext> where TDbContext : SxDbContext
    {
        public override SxBannerGroup[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(@" FROM D_BANNER_GROUP AS dbg ");

            object param = null;
            var gws = getBannerGroupWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dbg.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append(@"SELECT COUNT(1) FROM D_BANNER_GROUP AS dbg ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBannerGroup>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getBannerGroupWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dbg.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dbg.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc = desc
            };

            return query;
        }

        public void AddBanner(Guid bannerGroupId, Guid bannerId)
        {
            var query = @"INSERT INTO D_BANNER_GROUP_LINK
VALUES
  (
    @bid,
    @bgid
  )";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, new { bid = bannerId, bgid = bannerGroupId });
            }
        }

        public void DeleteBanner(Guid bannerGroupId, Guid bannerId)
        {
            var query = @"DELETE 
FROM   D_BANNER_GROUP_LINK
WHERE  BannerId = @bid
       AND BannerGroupId = @bgid";

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, new { bid = bannerId, bgid = bannerGroupId });
            }
        }
    }
}
