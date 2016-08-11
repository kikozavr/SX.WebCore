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
    public class SxRepoPicture<TDbContext> : SxDbRepository<Guid, SxPicture, TDbContext> where TDbContext : SxDbContext
    {
        public override SxPicture[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] {
                "dp.Id",
                "dp.Caption",
                "dp.[Description]",
                "dp.Width",
                "dp.Height",
                "dp.Size"
            }));
            sb.Append(" FROM D_PICTURE AS dp ");

            object param = null;
            var gws = getPicturesWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_PICTURE AS dp ");
            sbCount.Append(gws);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxPicture>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = conn.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getPicturesWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dp.Caption LIKE '%'+@caption+'%' OR @caption IS NULL)");
            query.Append(" AND (dp.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL)");
            query.Append(" AND (dp.Width >=@w OR @w=0)");
            query.Append(" AND (dp.Height >=@h OR @h=0)");

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

            return query.ToString();
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
