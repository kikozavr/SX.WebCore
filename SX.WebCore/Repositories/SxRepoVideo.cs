using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public class SxRepoVideo<TDbContext> : SxDbRepository<Guid, SxVideo, TDbContext, SxVMVideo> where TDbContext : SxDbContext
    {
        public override SxVMVideo[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_VIDEO AS dv ");

            object param = null;
            var gws = getVideoWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dv.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_VIDEO AS dv ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxVMVideo>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }
        private static string getVideoWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dv.VideoId LIKE '%'+@vid+'%' OR @vid IS NULL) ");

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var vid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.VideoId != null ? (string)filter.WhereExpressionObject.VideoId : null;

            param = new
            {
                title = title,
                vid = vid
            };

            return query.ToString();
        }

        public virtual SxVMVideo[] LinkedVideos(SxFilter filter, bool forMaterial)
        {
            var sb=new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString(new string[] { "dv.*" }));
            sb.AppendFormat(@" FROM D_VIDEO AS dv {0} JOIN D_VIDEO_LINK AS dvl ON dvl.VideoId = dv.Id ", forMaterial ? "" : @"LEFT");

            object param = null;
            sb.Append(getLinkedVideoWhereString(filter, forMaterial, out param));

            var defaultOrder = new SxOrder { FieldName = "dv.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));
            sb.Append(" OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY");

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVMVideo>(sql: sb.ToString(), param: param);
                return data.ToArray();
            }
        }

        public virtual int LinkedVideosCount(SxFilter filter, bool forMaterial)
        {
            var query = @"SELECT COUNT(1) FROM D_VIDEO AS dv " + (forMaterial ? "" : @"LEFT") + @" JOIN D_VIDEO_LINK AS dvl ON dvl.VideoId = dv.Id ";

            object param = null;
            query += getLinkedVideoWhereString(filter, forMaterial, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<int>(query, param: param).Single();
            }
        }

        private static void checkLinkedVideosFilter(SxFilter filter)
        {
            if(filter.AddintionalInfo==null)
                throw new ArgumentNullException("AddintionalInfo");
            if (filter.AddintionalInfo[0]==null)
                throw new ArgumentNullException("AddintionalInfo.MaterialId");
            if (filter.AddintionalInfo[1]==null)
                throw new ArgumentNullException("AddintionalInfo.ModelCoreType");
        }

        private static string getLinkedVideoWhereString(SxFilter filter, bool forMaterial, out object param)
        {
            checkLinkedVideosFilter(filter);

            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dv.VideoId LIKE '%'+@vid+'%' OR @vid IS NULL) ");
            if (forMaterial)
            {
                query.Append(" AND (dvl.ModelCoreType = @mct) ");
                query.Append(" AND (dvl.Materialid = @mid) ");
            }
            else
                query.Append(" AND (dv.Id NOT IN (SELECT dvl2.VideoId FROM D_VIDEO_LINK AS dvl2 WHERE dvl2.MaterialId=@mid AND dvl2.ModelCoreType=@mct)) ");


            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var vid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.VideoId != null ? (string)filter.WhereExpressionObject.VideoId : null;

            param = new
            {
                title = title,
                vid = vid,
                mid = filter.AddintionalInfo[0],
                mct = filter.AddintionalInfo[1]
            };

            return query.ToString();
        }

        public virtual void AddMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            var query = @"INSERT INTO D_VIDEO_LINK
VALUES
(
	@mid,
	@mct,
	@vid
)";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, vid = vid });
            }
        }

        public virtual void DeleteMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            var query = @"DELETE FROM D_VIDEO_LINK
WHERE MaterialId=@mid AND ModelCoreType=@mct AND VideoId=@vid";
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute(query, param: new { mid = mid, mct = mct, vid = vid });
            }
        }

        public override void Delete(SxVideo model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("dbo.del_video @videoId", new { videoId = model.Id });
            }
        }
    }
}
