using Dapper;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepoBannerGroup : SxDbRepository<Guid, SxBannerGroup, SxVMBannerGroup>
    {
        public override SxVMBannerGroup[] Read(SxFilter filter)
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

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMBannerGroup>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
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

        public override void Delete(SxBannerGroup model)
        {
            var query = @"DELETE FROM D_BANNER_GROUP WHERE Id=@bannerGroupId";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { bannerGroupId=model.Id });
            }
        }

        public override SxVMBannerGroup[] All
        {
            get
            {
                var query = @"SELECT*FROM D_BANNER_GROUP AS dbg";
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<SxVMBannerGroup>(query);
                    return data.ToArray();
                }
            }
        }
    }
}
