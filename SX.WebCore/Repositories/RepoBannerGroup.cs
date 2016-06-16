using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBannerGroup<TDbContext> : SxDbRepository<Guid, SxBannerGroup, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxBannerGroup> Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += " FROM D_BANNER_GROUP AS dbg ";

            object param = null;
            query += getBannerGroupWhereString(filter, out param);

            query += SxQueryProvider.GetOrderString("dbg.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBannerGroup>(query, param: param);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_BANNER_GROUP AS dbg ";

            object param = null;
            query += getBannerGroupWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
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
