using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoPicture<TDbContext> : SxDbRepository<Guid, SxPicture, TDbContext> where TDbContext : SxDbContext
    {
        public override SxVMPicture[] Query<SxVMPicture>(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dp.Id", "dp.Caption", "dp.[Description]", "dp.Width", "dp.Height", "dp.Size" });
            query += " FROM D_PICTURE AS dp";

            object param = null;
            query += getPictureWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMPicture>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_PICTURE as dp";

            object param = null;
            query += getPictureWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getPictureWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dp.Caption LIKE '%'+@caption+'%' OR @caption IS NULL)";
            query += " AND (dp.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL)";
            query += " AND (dp.Width >=@w OR @w=0)";
            query += " AND (dp.Height >=@h OR @h=0)";

            var caption = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Caption != null ? (string)filter.WhereExpressionObject.Caption : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;
            var w = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Width != null ? (int)filter.WhereExpressionObject.Width : 0;
            var h = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Height != null ? (int)filter.WhereExpressionObject.Height : 0;

            param = new
            {
                caption = caption,
                desc = desc,
                w = w,
                h = h
            };

            return query;
        }

        public override SxPicture GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<SxPicture>("dbo.get_picture @pictureId", new { pictureId = id[0] }).SingleOrDefault();
            }
        }

        public override void Delete(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_picture @pictureId", new { pictureId = id[0] });
            }
        }
    }
}
